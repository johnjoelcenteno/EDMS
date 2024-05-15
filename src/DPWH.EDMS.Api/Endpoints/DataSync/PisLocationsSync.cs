using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.Addresses.Commands.SyncAddress;
using DPWH.EDMS.Application.Features.DataSync.Commands;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Models.DpwhResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.DataSync;

public static class PisLocationsSync
{
    public static IEndpointRouteBuilder MapPisLocationsSyncEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.DataSync.PisLocations, Handle)
            .WithName(nameof(PisLocationsSync))
            .WithTags(DataSyncEndpointExtensions.Tag)
            .WithDescription("Synchronizes location data from PIS to system.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithRequestTimeout(TimeSpan.FromMinutes(10));

        return app;
    }

    private static async Task<IResult> Handle(IDpwhApiService apiService, ISender sender, CancellationToken cancellationToken)
    {
        try
        {
            var apiResponse = await apiService.GetLocationWithRetry<GeoRegionResponse>(GeoRegionResponse.Operation);

            if (apiResponse is null || apiResponse.GetLocations().Length == 0)
            {
                const string message = "Empty response from PIS Api";
                await sender.Send(new AddDataSyncLog("Locations", false, message), cancellationToken);
                var problemDetail = new ProblemDetails
                {
                    Title = "Unable to sync locations data.",
                    Detail = message,
                };

                return Results.NotFound(problemDetail);
            }

            var recordCount = await sender.Send(new SyncAddressCommand(apiResponse), cancellationToken);
            await sender.Send(new AddDataSyncLog("Locations", true, null), cancellationToken);

            var response = new BaseApiResponse<string>($"Retrieved and saved `{recordCount:N0}` locations.");

            return Results.Ok(response);
        }
        catch (Exception exception)
        {
            await sender.Send(new AddDataSyncLog("Locations", false, exception.Message), cancellationToken);
            throw;
        }
    }
}