using DPWH.EDMS.Application.Features.Lookups.Queries;
using DPWH.EDMS.Application.Features.Lookups.Queries.GetPuposes;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class PurposesEndpoint
{
    public const string PurposeCacheTag = "PurposeCacheTag";
    public static IEndpointRouteBuilder MapPurposes(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.Purposes, async (IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetPurposesQuery(), token);

                var data = new BaseApiResponse<IEnumerable<GetLookupResult>>(result);

                return Results.Ok(data);
            })
            .WithName("GetPurposes")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetLookupResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(PurposeCacheTag));

        return app;
    }
}