namespace DPWH.EDMS.Api.Endpoints.Root;
public static class RootEndpointExtensions
{
    public static IEndpointRouteBuilder MapRootEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRootEndpoint();
        return app;
    }
}
