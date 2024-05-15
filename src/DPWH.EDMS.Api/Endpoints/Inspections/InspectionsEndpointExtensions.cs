namespace DPWH.EDMS.Api.Endpoints.Inspections;

public static class InspectionsEndpointExtensions
{
    public static IEndpointRouteBuilder MapInspectionsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapInspections();
        app.MapInspectionsImages();
        app.MapRentalRatesProperty();
        app.MapRentalRates();
        app.MapRentalRatesDocument();

        return app;
    }
}
