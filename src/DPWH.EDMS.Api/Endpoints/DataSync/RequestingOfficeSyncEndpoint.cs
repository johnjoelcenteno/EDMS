using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Api.Endpoints.Lookups;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.RequestingOffices.Commands.BatchCreateRequestingOffice;
using DPWH.EDMS.Application.Models.DpwhResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace DPWH.EDMS.Api.Endpoints.DataSync;

public static class RequestingOfficeSyncEndpoint
{
    public static IEndpointRouteBuilder MapRequestingOfficeSync(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.DataSync.RequestingOffices, async (bool? enableCleanUp, IDpwhApiService dpwhApiService,
            IOutputCacheStore cache, IMediator mediator, CancellationToken token) =>
            {
                var requestingOffices = await dpwhApiService.GetWithRetry<RequestingOfficeResponse>(RequestingOfficeResponse.Operation);

                await mediator.Send(new BatchCreateRequestingOfficeCommand(requestingOffices, enableCleanUp ?? true), token);
                await cache.EvictByTagAsync(RequestingOfficeEndpoint.RequestingOfficeCacheTag, token);
            })
            .WithName("RequestingOfficeSync")
            .WithTags(DataSyncEndpointExtensions.Tag)
            .WithDescription("Sync requesting offices from PIS Api result to App Db. Clean up before sync is enabled by default.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}