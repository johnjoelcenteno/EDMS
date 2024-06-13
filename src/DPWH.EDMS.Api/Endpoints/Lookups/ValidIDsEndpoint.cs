using DPWH.EDMS.Application.Features.Lookups.Queries.GetValidIDs;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class ValidIDsEndpoint
{
    public const string ValidIDsCacheTag = "ValidIDsCacheTag";

    public static IEndpointRouteBuilder MapValidIDs(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.ValidIDs, async (IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetValidIDsQuery(), token);

                var data = new BaseApiResponse<IEnumerable<GetValidIDsResult>>(result);

                return Results.Ok(data);
            })
            .WithName("GetValidIDs")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetValidIDsResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(ValidIDsCacheTag));

        return app;
    }
}