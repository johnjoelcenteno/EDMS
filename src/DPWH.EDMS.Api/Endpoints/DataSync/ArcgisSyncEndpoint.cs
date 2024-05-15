using DPWH.EDMS.Application.Features.ArcGis.Commands.BatchCreateArcgis;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.DataSync;

public static class ArcgisSyncEndpoint
{
    public static IEndpointRouteBuilder MapArcgisSync(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.DataSync.ArcgisSync, async (bool EnableCleanup, string serviceName, int layerId, string regionId, IMediator mediator, CancellationToken token) =>
        {
            var command = new BatchCreateArcgisCommand(EnableCleanup, serviceName, layerId, regionId);
            await mediator.Send(command, token);
        })
             .WithName("ArcgisSync")
            .WithTags(DataSyncEndpointExtensions.Tag)
            .WithDescription("Sync agencies along with its mapped attached agencies from PIS Api result to App Db. Clean up before sync is enabled by default.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError); ;

        return app;
    }


}

