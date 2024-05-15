using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DPWH.EDMS.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<GetUserByIdResult>;

internal sealed class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
{
    private readonly ILogger<GetUserByIdHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdHandler(ILogger<GetUserByIdHandler> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user is not null)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var roleClaim = claims.FirstOrDefault(c => c.Type == "role");

            return new GetUserByIdResult
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
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
                CreatedBy = user.CreatedBy,
                CreatedDate = user.Created
            };
        }

        _logger.LogError("User with id `{UserId}` not found", request.UserId);
        throw new AppException("User not found");
    }
}