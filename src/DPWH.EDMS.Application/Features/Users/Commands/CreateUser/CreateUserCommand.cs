using System.Security.Claims;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DPWH.EDMS.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<CreateUserResult>
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string MiddleInitial { get; set; }
    public required string LastName { get; set; }
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

internal sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    private const string DefaultPassword = "P@ssw0rd";

    private readonly ILogger<CreateUserHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ClaimsPrincipal _principal;

    public CreateUserHandler(
        ILogger<CreateUserHandler> logger,
        UserManager<ApplicationUser> userManager,
        ClaimsPrincipal principal)
    {
        _logger = logger;
        _userManager = userManager;
        _principal = principal;
    }

    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
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

        var user = ApplicationUser.Create(
              command.Email,
              command.Email,
              command.MobileNumber,
              basicInfo,
              employeeInfo,
              _principal.GetUserName()
            );

        var result = await _userManager.CreateAsync(user, DefaultPassword);

        if (result.Succeeded)
        {
            return new CreateUserResult
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                FirstName = user.UserBasicInfo?.FirstName,
                MiddleInitial = user.UserBasicInfo?.MiddleInitial,
                LastName = user.UserBasicInfo?.LastName,
                MobileNumber = user.PhoneNumber,
                Department = user.EmployeeInfo?.Department,
                Position = user.EmployeeInfo?.Position,
                RegionalOfficeRegion = user.EmployeeInfo?.RegionalOfficeRegion,
                RegionalOfficeProvince = user.EmployeeInfo?.RegionalOfficeProvince,
                DistrictEngineeringOffice = user.EmployeeInfo?.DistrictEngineeringOffice,
                DesignationTitle = user.EmployeeInfo?.DesignationTitle,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.Created
            };
        }

        var error = result.Errors.First().Description;
        _logger.LogError("Failed creating user `{Email}`: {Error}", command.Email, error);
        throw new AppException($"Failed creating user `{command.Email}`: {error}");
    }
}