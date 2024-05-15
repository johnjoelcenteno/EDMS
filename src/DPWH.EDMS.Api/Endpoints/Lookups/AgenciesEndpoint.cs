using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Features.Agencies.Queries.GetAgencies;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class AgenciesEndpoint
{
    public const string AgencyCacheTag = "AgencyCacheTag";

    public static IEndpointRouteBuilder MapAgencies(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.Agencies, async (IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAgenciesQuery(), token);
                var data = new BaseApiResponse<IEnumerable<GetAgenciesResult>>(result);

                return Results.Ok(data);
            })
            .WithName("GetAgencyList")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetAgenciesResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(AgencyCacheTag));

        return app;
    }
}