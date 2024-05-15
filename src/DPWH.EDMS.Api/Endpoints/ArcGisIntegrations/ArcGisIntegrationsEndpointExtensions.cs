namespace DPWH.EDMS.Api.Endpoints.ArcGisIntegrations;

public static class ArcGisIntegrationsEndpointExtensions
{
    public static IEndpointRouteBuilder MapArcGisIntegrationsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapArcGisIntegrations();

        return app;
    }
}