using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Features.Lookups.Models;
using DPWH.EDMS.Application.Models;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class LookupsEndpoint
{
    public static IEndpointRouteBuilder MapLookups(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.CommonPropertyType, () =>
            {
                var lookup = new CommonLookup(
                    new[] {
                        new Lookup(
                            "PropertyTypes",
                            new List<SimpleKeyValue> {
                                new("Multiple Use", "CODE 0 - Multiple Use"),
                                new("Residential", "CODE 1 - Residential"),
                                new("Open Space", "CODE 2 - Open Space"),
                                new("Commercial", "CODE 3 - Commercial"),
                                new("Industrial", "CODE 4 - Industrial"),
                                new("Personal Property", "CODE 5 - Personal Property"),
                                new("Forest Property", "CODE 6 - Forest Property"),
                                new("Agricultural/Horticultural", "CODE 7 - Agricultural/Horticultural"),
                                new("Recreational Property", "CODE 8 - Recreational Property"),
                                new("Exempt Property", "CODE 9 - Exempt Property") })},
                    Enumerable.Empty<AddressLookup>());

                return Results.Ok(lookup);
            })
            .WithName(CommonLookupsEndpointExtensions.Tag)
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<CommonLookup>()
            .CacheOutput();

        return app;
    }
}