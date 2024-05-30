using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetUsersHandler(IReadAppIdpRepository repository) : IRequestHandler<GetUsersQuery, DataSourceResult>
{
    public Task<DataSourceResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = repository
            .ViewUserAccess
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
                Created = p.Created,
                CreatedBy = p.CreatedBy
            })
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}