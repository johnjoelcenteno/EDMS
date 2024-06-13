using DPWH.EDMS.Application.Features.Lookups.Queries.GetRecordTypes;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class RecordTypesEndpoint
{
    public const string RecordTypeCacheTag = "RecordTypeCacheTag";

    public static IEndpointRouteBuilder MapRecordTypes(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.RecordTypes, async (IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetRecordTypesQuery(), token);

                var data = new BaseApiResponse<IEnumerable<GetRecordTypesResult>>(result);

                return Results.Ok(data);
            })
            .WithName("GetRecordTypes")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetRecordTypesResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(RecordTypeCacheTag));

        return app;
    }
}