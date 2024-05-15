namespace DPWH.EDMS.Api.Endpoints.Licenses;
public static class LicensesEndpointExtensions
{
    public static IEndpointRouteBuilder MapLicensesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapLicenses();
        return app;
    }
}