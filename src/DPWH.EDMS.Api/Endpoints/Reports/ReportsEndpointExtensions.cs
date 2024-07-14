namespace DPWH.EDMS.Api.Endpoints.Reports;

public static class ReportsEndpointExentsions
{
    public static IEndpointRouteBuilder MapReportsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapReports();
        return app;
    }
}