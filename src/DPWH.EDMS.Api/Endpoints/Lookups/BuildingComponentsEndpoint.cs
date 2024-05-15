using DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetBuildingComponents;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class BuildingComponentsEndpoint
{
    public static IEndpointRouteBuilder MapBuildingComponents(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.BuildingComponents, async (IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetBuildingComponents(), token);
                var data = new BaseApiResponse<IEnumerable<GetBuildingComponentsResult>>(result);

                return Results.Ok(data);
            })
            .WithName("GetBuildingComponents")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithDescription("Get building components")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetBuildingComponentsResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        return app;
    }
}