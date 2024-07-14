namespace DPWH.EDMS.Api.Endpoints.DataSync;

public static class DataSyncEndpointExtensions
{
    public const string Tag = "DataSync";

    public static IEndpointRouteBuilder MapDataSyncEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapRequestingOfficeSync();
        app.MapGeoRegionSync();
        app.MapEmployeeSync();
        app.MapAgenciesSync();        
        app.MapPisLocationsSyncEndpoint();

        return app;
    }
}