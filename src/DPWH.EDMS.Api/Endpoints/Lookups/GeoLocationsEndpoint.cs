using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Features.Lookups.Models;
using DPWH.EDMS.Application.Features.Lookups.Queries.GetBarangays;
using DPWH.EDMS.Application.Features.Lookups.Queries.GetCities;
using DPWH.EDMS.Application.Features.Lookups.Queries.GetProvinces;
using DPWH.EDMS.Application.Features.Lookups.Queries.GetRegions;
using MediatR;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class GeoLocationsEndpoint
{
    public static IEndpointRouteBuilder MapGeoLocations(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.Regions, async (IMediator mediator, CancellationToken token) =>
            {
                var request = new GetRegionsQuery();
                var response = await mediator.Send(request, token);

                return response.AddressData.Any() ? Results.Ok(response) : Results.NotFound();
            })
            .WithName("GetRegions")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<AddressLookup>()
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(15)));

        app.MapGet(ApiEndpoints.Lookups.Province, async (IMediator mediator, string regionCode, CancellationToken token) =>
            {
                var request = new GetProvincesQuery(regionCode);
                var response = await mediator.Send(request, token);

                return response.AddressData.Any() ? Results.Ok(response) : Results.NotFound();
            })
            .WithName("GetProvinces")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<AddressLookup>()
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(15)));

        app.MapGet(ApiEndpoints.Lookups.CityOrMunicipality, async (IMediator mediator, string provinceCode, CancellationToken token) =>
            {
                var request = new GetCitiesQuery(provinceCode);
                var response = await mediator.Send(request, token);

                return response.AddressData.Any() ? Results.Ok(response) : Results.NotFound();
            })
            .WithName("GetCityOrMunicipalities")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<AddressLookup>()
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(15)));

        app.MapGet(ApiEndpoints.Lookups.Barangay, async (IMediator mediator, string cityOrMunicipalityCode, CancellationToken token) =>
            {
                var request = new GetBarangaysQuery(cityOrMunicipalityCode);
                var response = await mediator.Send(request, token);

                return response.AddressData.Any() ? Results.Ok(response) : Results.NotFound();
            })
            .WithName("GetBarangays")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<AddressLookup>()
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(15)));

        return app;
    }
}
