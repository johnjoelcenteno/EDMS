namespace DPWH.EDMS.Api.Endpoints.Roles;

public static class RolesEndpointExtensions
{
    public static IEndpointRouteBuilder MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRoles();
        return app;
    }
}
