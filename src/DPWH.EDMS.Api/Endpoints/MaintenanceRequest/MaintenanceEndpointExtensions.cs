namespace DPWH.EDMS.Api.Endpoints.MaintenanceRequest;

public static class MaintenanceEndpointExtensions
{
    public static IEndpointRouteBuilder MapMaintenanceRequestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapMaintenanceRequest();
        app.MapMaintenanceRequestDocuments();
        return app;
    }
}
