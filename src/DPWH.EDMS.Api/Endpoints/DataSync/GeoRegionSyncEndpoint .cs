using DPWH.EDMS.Application.Features.DataSync.Queries;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.DataSync;

public static class GeoRegionSyncEndpoint
{
    public static IEndpointRouteBuilder MapGeoRegionSync(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.DataSync.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var syncedData = new GetDataSyncQuery(request);
            var dataSourceResult = await mediator.Send(syncedData, cancellationToken);

            return Results.Ok(dataSourceResult);
        })
            .WithName("QuerySyncedItems")
            .WithTags(DataSyncEndpointExtensions.Tag)
            .WithDescription("Retrieves the synced data")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}