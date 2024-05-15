namespace DPWH.EDMS.Api.Endpoints.DpwhIntegrations;

public static class DpwhIntegrationsEndpointExtensions
{
    public static IEndpointRouteBuilder MapDpwhIntegrationsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDpwhIntegrations();

        return app;
    }
}