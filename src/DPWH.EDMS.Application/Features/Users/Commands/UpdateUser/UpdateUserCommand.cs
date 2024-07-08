using System.Security.Claims;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DPWH.EDMS.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand : IRequest<UpdateUserResult>
{
    public required Guid UserId { get; set; }
    public required string FirstName { get; set; }
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

internal sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly ILogger<UpdateUserHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ClaimsPrincipal _principal;

    public UpdateUserHandler(
        ILogger<UpdateUserHandler> logger,
        UserManager<ApplicationUser> userManager,
        ClaimsPrincipal principal)
    {
        _logger = logger;
        _userManager = userManager;
        _principal = principal;
    }

    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
        {
            _logger.LogError("User `{UserId}` not found", command.UserId);
            throw new AppException($"User `{command.UserId}` not found");
        }

        var userBasicInfo = UserBasicInfo.Create(
            command.FirstName,
            null,
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

        var updateResult = await _userManager.UpdateAsync(user);

        if (updateResult.Succeeded)
        {
            //Getting claims of User
            var claims = await _userManager.GetClaimsAsync(user);

            //Removing Existing Role
            var roleClaims = claims.Where(x => x.Type == "role");
            if (roleClaims.Any())
            {
                await _userManager.RemoveClaimsAsync(user, roleClaims);
            }
            //Adding new Role
            var addClaimResult = await _userManager.AddClaimAsync(user, new Claim("role", command.Role));

            if (addClaimResult.Succeeded)
            {
                return new UpdateUserResult
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    FirstName = user.UserBasicInfo?.FirstName,
                    LastName = user.UserBasicInfo?.LastName,
                    OldAccess = ApplicationRoles.GetDisplayRoleName(roleClaims.FirstOrDefault()?.Value, "None"),
                    Role = command.Role,
                    UserAccess = ApplicationRoles.GetDisplayRoleName(command.Role),
                    MobileNumber = user.PhoneNumber,
                    Department = user.EmployeeInfo?.Department,
                    Position = user.EmployeeInfo?.Position,
                    RegionalOfficeRegion = user.EmployeeInfo?.RegionalOfficeRegion,
                    RegionalOfficeProvince = user.EmployeeInfo?.RegionalOfficeProvince,
                    DistrictEngineeringOffice = user.EmployeeInfo?.DistrictEngineeringOffice,
                    CreatedBy = user.CreatedBy,
                    CreatedDate = user.Created,
                    LastModifiedBy = user.LastModifiedBy!,
                    LastModified = user.LastModified!.Value
                };
            }

            var addClaimError = addClaimResult.Errors.First().Description;
            _logger.LogError("Failed to set role `{Role}` for user `{Email}`: {Error}", command.Role, user.Email, addClaimError);
            throw new AppException($"Failed to set role `{command.Role}` for user `{user.Email}`: {addClaimError}");
        }

        var userUpdateError = updateResult.Errors.First().Description;
        _logger.LogError("Failed to update user `{Email}`: {Error}", user.Email, userUpdateError);
        throw new AppException($"Failed to update user `{user.Email}`: {userUpdateError}");
    }
}