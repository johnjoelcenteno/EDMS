namespace DPWH.EDMS.Api.Endpoints.Users;

public static class UsersEndpointExtensions
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapUsers();

        return app;
    }
}