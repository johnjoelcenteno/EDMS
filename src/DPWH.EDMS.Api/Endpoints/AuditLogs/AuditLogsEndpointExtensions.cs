namespace DPWH.EDMS.Api.Endpoints.AuditLogs;

public static class AuditLogsEndpointExtensions
{
    public static IEndpointRouteBuilder MapAuditLogsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuditLogs();
        return app;
    }
}
