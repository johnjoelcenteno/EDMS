using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DPWH.EDMS.Application.Features.Users.Queries.GetUserById;

public record GetUserByEmployeeId(string EmployeeId) : IRequest<GetUserByIdResult>;

internal class GetUserByEmployeeIdHandler : IRequestHandler<GetUserByEmployeeId, GetUserByIdResult>
{
    private readonly ILogger<GetUserByEmployeeIdHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IReadAppIdpRepository _repository;

    public GetUserByEmployeeIdHandler(ILogger<GetUserByEmployeeIdHandler> logger, UserManager<ApplicationUser> userManager, IReadAppIdpRepository repository)
    {
        _logger = logger;
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<GetUserByIdResult> Handle(GetUserByEmployeeId request, CancellationToken cancellationToken)
    {
        var user = await _repository
            .UsersView
            .FirstOrDefaultAsync(x => x.EmployeeInfo.EmployeeId == request.EmployeeId, cancellationToken);

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

        _logger.LogError("User with id `{EmployeeId}` not found", request.EmployeeId);
        throw new AppException("User not found");
    }
}
