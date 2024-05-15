namespace DPWH.EDMS.Api.Endpoints.ProjectMonitoring;

public static class ProjectMonitoringEndpointExtensions
{
    public static IEndpointRouteBuilder MapProjectMonitoringEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapProjectMonitoring();
        app.MapProjectMonitoringDocuments();
        return app;
    }
}
