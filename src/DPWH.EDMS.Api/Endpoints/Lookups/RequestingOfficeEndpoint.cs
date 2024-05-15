using DPWH.EDMS.Application.Features.RequestingOffices.Queries.GetRequestingOffices;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class RequestingOfficeEndpoint
{
    public const string RequestingOfficeCacheTag = "RequestingOfficeCacheTag";

    public static IEndpointRouteBuilder MapRequestingOfficeLookups(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.RequestingOffices, async (IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetRequestingOfficesQuery(), token);
                var data = new BaseApiResponse<IEnumerable<GetRequestingOfficeResult>>(result);

                return Results.Ok(data);
            })
            .WithName("GetRequestingOfficeList")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetRequestingOfficeResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(RequestingOfficeCacheTag));

        return app;
    }
}