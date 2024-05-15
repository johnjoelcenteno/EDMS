namespace DPWH.EDMS.Api.Endpoints.Assets;

public static class AssetsEndpointExtensions
{
    public static IEndpointRouteBuilder MapAssetEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAssets();
        app.MapAssetDocuments();

        return app;
    }
}