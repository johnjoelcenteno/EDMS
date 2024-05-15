using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DPWH.EDMS.Application.Features.Users.Commands.RemoveUser;

public record RemoveUserCommand(Guid UserId) : IRequest<RemoveUserResult>;

internal sealed class RemoveUserHandler : IRequestHandler<RemoveUserCommand, RemoveUserResult>
{
    private readonly ILogger<RemoveUserHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public RemoveUserHandler(ILogger<RemoveUserHandler> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<RemoveUserResult> Handle(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());

        if (user is null)
        {
            _logger.LogError("User `{UserId}` not found", command.UserId);
            throw new AppException($"User `{command.UserId}` not found");
        }

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            return new RemoveUserResult
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                FirstName = user.UserBasicInfo?.FirstName,
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

        _logger.LogError("Unable to remove user `{UserId}`", command.UserId);
        throw new AppException($"Unable to remove user `{command.UserId}`");
    }
}