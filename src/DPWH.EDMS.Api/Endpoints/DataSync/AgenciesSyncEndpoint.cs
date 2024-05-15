using DPWH.EDMS.Api.Endpoints.Lookups;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.Agencies.Commands.BatchCreateAgencies;
using DPWH.EDMS.Application.Models.DpwhResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace DPWH.EDMS.Api.Endpoints.DataSync;

public static class AgenciesSyncEndpoint
{
    public static IEndpointRouteBuilder MapAgenciesSync(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.DataSync.Agencies, async (
                bool? enableCleanUp,
                IDpwhApiService dpwhApiService,
                IOutputCacheStore cache,
                IMediator mediator,
                CancellationToken token) =>
            {
                var nationalGovtAgencies = await dpwhApiService.GetWithRetry<NationalGovtAgencyResponse>(NationalGovtAgencyResponse.Operation);

                await mediator.Send(new BatchCreateAgenciesCommand(nationalGovtAgencies, enableCleanUp), token);
                await cache.EvictByTagAsync(AgenciesEndpoint.AgencyCacheTag, token);
            })
            .WithName("AgenciesSync")
            .WithTags(DataSyncEndpointExtensions.Tag)
            .WithDescription("Sync agencies along with its mapped attached agencies from PIS Api result to App Db. Clean up before sync is enabled by default.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}