using System.Linq.Expressions;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.IDP.Core.Constants;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DPWH.EDMS.Application.Features.Roles.Queries.GetRoles;

public record GetRolesQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal class GetRolesHandler : IRequestHandler<GetRolesQuery, DataSourceResult>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public GetRolesHandler(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public Task<DataSourceResult> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = _roleManager
            .Roles
            .Select(x => MapToModel(x))
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(roles);
    }

    private static RoleModel MapToModel(IdentityRole c)
    {
        var func = MapToModelExpression().Compile();
        return func(c);
    }

    private static Expression<Func<IdentityRole, RoleModel>> MapToModelExpression()
    {
        return c => new RoleModel
        {
            Id = Guid.Parse(c.Id),
            Name = c.Name,
            UserAccess = ApplicationRoles.GetDisplayRoleName(c.Name, "None")
        };
    }
}
