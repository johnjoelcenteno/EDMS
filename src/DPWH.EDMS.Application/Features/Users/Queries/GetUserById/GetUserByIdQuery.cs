using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DPWH.EDMS.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<GetUserByIdResult>;

internal sealed class GetUserByIdHandler(
    ILogger<GetUserByIdHandler> logger,
    UserManager<ApplicationUser> userManager,
    IReadAppIdpRepository repository)
    : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
{
    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.UsersView
            .FirstOrDefaultAsync(u => u.Id == request.UserId.ToString(), cancellationToken);

        if (user is null)
        {
            logger.LogError("User with id `{UserId}` not found", request.UserId);
            throw new AppException("User not found");
        }

        var claims = await userManager.GetClaimsAsync(user);
        var roleClaim = claims.FirstOrDefault(c => c.Type == "role");

        return new GetUserByIdResult
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            EmployeeId = user.EmployeeInfo?.EmployeeId,
            FirstName = user.UserBasicInfo?.FirstName,
            MiddleInitial = user.UserBasicInfo?.MiddleInitial,
            LastName = user.UserBasicInfo?.LastName,
            MobileNumber = user.PhoneNumber,
            Role = roleClaim?.Value,
            UserAccess = ApplicationRoles.GetDisplayRoleName(roleClaim?.Value),
            Department = user.EmployeeInfo?.Department,
            Position = user.EmployeeInfo?.Position,
            RegionalOfficeRegion = user.EmployeeInfo?.RegionalOfficeRegion,
            RegionalOfficeProvince = user.EmployeeInfo?.RegionalOfficeProvince,
            DistrictEngineeringOffice = user.EmployeeInfo?.DistrictEngineeringOffice,
            DesignationTitle = user.EmployeeInfo?.DesignationTitle,
            Office = user.EmployeeInfo?.Office,
            CreatedBy = user.CreatedBy,
            CreatedDate = user.Created
        };
    }
}