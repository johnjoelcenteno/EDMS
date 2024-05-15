namespace DPWH.EDMS.Api.Endpoints.SystemLogs;

public static class SystemLogsEndpointExtensions
{
    public static IEndpointRouteBuilder MapSystemLogsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapSystemLogs();
        return app;
    }
}