using System.Security.Claims;
using DPWH.EDMS.Application.Services;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DPWH.EDMS.Application.Features.Users.Commands.CreateUserWithRole;

public record CreateUserWithRoleCommand : IRequest<CreateUserWithRoleResult>
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string MiddleInitial { get; set; }
    public required string LastName { get; set; }
    public required string Role { get; set; }
    public string? MobileNumber { get; set; }
    public string? EmployeeId { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? RegionalOfficeRegion { get; set; }
    public string? RegionalOfficeProvince { get; set; }
    public string? DistrictEngineeringOffice { get; set; }
    public string? DesignationTitle { get; set; }
    public string? Office { get; set; }
}

internal sealed class CreateUserWithRoleHandler : IRequestHandler<CreateUserWithRoleCommand, CreateUserWithRoleResult>
{
    private const string LoginProvider = "AAD";

    private readonly ILogger<CreateUserWithRoleHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserAccessLevelService _userAccessLevelService;
    private readonly ClaimsPrincipal _principal;

    public CreateUserWithRoleHandler(
        ILogger<CreateUserWithRoleHandler> logger,
        UserManager<ApplicationUser> userManager,
        IUserAccessLevelService userAccessLevelService,
        ClaimsPrincipal principal)
    {
        _logger = logger;
        _userManager = userManager;
        _userAccessLevelService = userAccessLevelService;
        _principal = principal;
    }

    public async Task<CreateUserWithRoleResult> Handle(CreateUserWithRoleCommand command, CancellationToken cancellationToken)
    {
        bool allowCreation;

        _logger.LogInformation("Creating new user with role {@CreateUserWithRoleRequest}", command);

        //check if user already exist            
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (user is not null)
        {
            //check if this is created via AutoProvision
            if (user.CreatedBy == "AutoProvision" && string.IsNullOrEmpty(user.EmployeeInfo.EmployeeId))
            {
                _logger.LogInformation("User created via auto AutoProvision so we need to update it");

                //update properties
                var userBasicInfo = UserBasicInfo.Create(
                    command.FirstName,
                    command.MiddleInitial,
                    command.LastName);

                var employeeInfo = EmployeeInfo.Create(
                    command.EmployeeId,
                    command.Department,
                    command.Position,
                    command.RegionalOfficeRegion,
                    command.RegionalOfficeProvince,
                    command.DistrictEngineeringOffice,
                    command.DesignationTitle,
                    command.Office);

                user.Update(
                    userBasicInfo,
                    employeeInfo,
                    _principal.GetUserName());

                await _userManager.UpdateAsync(user);

                //update/add claims
                var userClaims = new List<Claim>();

                var roleClaim = new Claim("role", command.Role);
                var clientClaim = new Claim("client", "EDMS");
                userClaims.Add(roleClaim);
                userClaims.Add(clientClaim);
                var addRoleClaimResult = await _userManager.AddClaimsAsync(user, userClaims);
                if (!addRoleClaimResult.Succeeded) throw new AppException(addRoleClaimResult.Errors.First().Description);

            }
            else
            {                
                /// Special handling here: 
                /// If user being added is already in User store but not yet part of EDMS - update it
                
                var claims = await _userManager.GetClaimsAsync(user);

                var edmsClaim = claims.FirstOrDefault(x => x.Type == "client" && x.Value == "EDMS");
                if (edmsClaim is null)
                {                    
                    var addClientClaimResult = await _userManager.AddClaimAsync(user, new Claim("client", "EDMS"));
                    var addRoleClaimResult = await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Role, command.Role));

                    return new CreateUserWithRoleResult
                    {
                        Id = user.Id,
                        UserName = user.UserName!,
                        Email = user.Email!,                        
                        Role = command.Role,
                        UserAccess = ApplicationRoles.GetDisplayRoleName(command.Role),
                        CreatedBy = user.CreatedBy
                    };
                }
                else
                {
                    _logger.LogError("User `{Email}` already exists", command.Email);
                    throw new AppException($"User `{command.Email}` already exists");
                }
            }            
        }

        if (ApplicationPolicies.NoLicenseUsers.Contains(command.Role))
        {
            allowCreation = true;
        }
        else
        {
            //check if we are allowed to add new user with license
            var licenseStatusResult = await _userAccessLevelService.GetLicenseStatus(cancellationToken);

            if (licenseStatusResult.Available > 0)
            {
                allowCreation = true;
            }
            else
            {
                _logger.LogError("No available license");
                throw new AppException("No available license");
            }
        }

        if (allowCreation)
        {
            var basicInfo = UserBasicInfo.Create(
                command.FirstName,
                command.MiddleInitial,
                command.LastName);
            var employeeInfo = EmployeeInfo.Create(
                command.EmployeeId,
                command.Department,
                command.Position,
                command.RegionalOfficeRegion,
                command.RegionalOfficeProvince,
                command.DistrictEngineeringOffice,
                command.DesignationTitle,
                command.Office);

            var newUser = ApplicationUser.Create(
                    command.Email,
                    command.Email,
                    command.MobileNumber,
                    basicInfo,
                    employeeInfo,
                    _principal.GetUserName()
                );

            var result = await _userManager.CreateAsync(newUser);
            if (result.Succeeded)
            {
                var userClaims = new List<Claim>();
                var roleClaim = new Claim(JwtClaimTypes.Role, command.Role);
                var nameClaim = new Claim(JwtClaimTypes.Name, command.FirstName + " " + command.LastName);
                var clientClaim = new Claim("client", "EDMS");
                userClaims.Add(roleClaim);
                userClaims.Add(nameClaim);
                userClaims.Add(clientClaim);

                var addRoleClaimResult = await _userManager.AddClaimsAsync(newUser, userClaims);

                var identityResult = await _userManager.AddLoginAsync(newUser, new UserLoginInfo(LoginProvider, newUser.Email, LoginProvider));
                if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

                if (addRoleClaimResult.Succeeded)
                {
                    return new CreateUserWithRoleResult
                    {
                        Id = newUser.Id,
                        UserName = newUser.UserName!,
                        Email = newUser.Email!,
                        FirstName = newUser.UserBasicInfo?.FirstName,
                        MiddleInitial = newUser.UserBasicInfo?.MiddleInitial,
                        LastName = newUser.UserBasicInfo?.LastName,
                        Role = command.Role,
                        UserAccess = ApplicationRoles.GetDisplayRoleName(command.Role),
                        MobileNumber = newUser.PhoneNumber,
                        Department = newUser.EmployeeInfo?.Department,
                        Position = newUser.EmployeeInfo?.Position,
                        RegionalOfficeRegion = newUser.EmployeeInfo?.RegionalOfficeRegion,
                        RegionalOfficeProvince = newUser.EmployeeInfo?.RegionalOfficeProvince,
                        DistrictEngineeringOffice = newUser.EmployeeInfo?.DistrictEngineeringOffice,
                        DesignationTitle = newUser.EmployeeInfo?.DesignationTitle,
                        CreatedBy = newUser.CreatedBy,
                        CreatedDate = newUser.Created
                    };
                }

                var roleCreationError = addRoleClaimResult.Errors.First().Description;
                _logger.LogError("Failed to add role to new user `{Email}`: {Error}", command.Email, roleCreationError);
                throw new AppException(roleCreationError);
            }

            var userCreationError = result.Errors.First().Description;
            _logger.LogError("Unable to create user `{Email}`: {Error}", command.Email, userCreationError);
            throw new AppException(userCreationError);
        }

        _logger.LogError("User creation not allowed");
        throw new AppException("User creation not allowed");
    }
}