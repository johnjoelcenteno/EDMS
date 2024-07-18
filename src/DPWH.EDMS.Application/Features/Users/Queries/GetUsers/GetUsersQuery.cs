using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using KendoNET.DynamicLinq;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetUsersHandler(IReadAppIdpRepository repository, ClaimsPrincipal claimsPrincipal) : IRequestHandler<GetUsersQuery, DataSourceResult>
{
    public Task<DataSourceResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = repository
            .ViewUserAccess.AsQueryable();

        var currentUserId = claimsPrincipal.GetUserId().ToString();

        var result = users
            .Where(p => ApplicationPolicies.RequireActiveRoles.Contains(p.UserRole) && 
                        !(p.UserRole == ApplicationRoles.ITSupport && 
                        p.Id == currentUserId))
            .Select(p => new GetUsersQueryResult
            {
                Id = p.Id,
                UserName = p.UserName,
                Email = p.Email,
                FirstName = p.FirstName,
                MiddleInitial = p.MiddleInitial,
                LastName = p.LastName,
                EmployeeId = p.EmployeeId,
                MobileNumber = p.MobileNumber,
                Role = p.UserRole,
                UserAccess = p.UserAccess,
                Department = p.Department,
                Position = p.Position,
                RegionalOfficeRegion = p.RegionalOfficeRegion,
                RegionalOfficeProvince = p.RegionalOfficeProvince,
                DistrictEngineeringOffice = p.DistrictEngineeringOffice,
                DesignationTitle = p.DesignationTitle,
                Office = p.Office,
                Created = p.Created,
                CreatedBy = p.CreatedBy
            })
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}