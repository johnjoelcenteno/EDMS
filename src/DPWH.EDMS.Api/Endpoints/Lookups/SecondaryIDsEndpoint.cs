using DPWH.EDMS.Application.Features.Lookups.Queries.GetSecondaryIDs;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class SecondaryIDsEndpoint
{
    public const string SecondaryIDCacheTag = "SecondaryIDCacheTag";
    public static IEndpointRouteBuilder MapSecondaryIDs(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.SecondaryIDs, async (IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetSecondaryIDsQuery(), token);

                var data = new BaseApiResponse<IEnumerable<GetSecondaryIDsResult>>(result);

                return Results.Ok(data);
            })
            .WithName("GetSecondaryIDs")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetSecondaryIDsResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(SecondaryIDCacheTag));

        return app;
    }
}