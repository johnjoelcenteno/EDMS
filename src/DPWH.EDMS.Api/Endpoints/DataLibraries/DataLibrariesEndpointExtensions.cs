namespace DPWH.EDMS.Api.Endpoints.DataLibraries;

public static class DataLibrariesEndpointExtensions
{
    public static IEndpointRouteBuilder MapDataLibrariesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDataLibraries();

        return app;
    }
}