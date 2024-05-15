using DPWH.EDMS.Application.Features.ArcGis.Commands.DeleteAllFeatures;
using DPWH.EDMS.Application.Features.ArcGis.Commands.DeleteFeatures;
using DPWH.EDMS.Application.Features.ArcGis.Commands.UpdateFeatures;
using DPWH.EDMS.Application.Features.ArcGis.Commands.UploadFeatures;
using DPWH.EDMS.Application.Features.ArcGis.Queries.FeatureServiceLayer;
using DPWH.EDMS.Application.Features.ArcGis.Queries.GetLayerMetadata;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.ArcGisIntegrations;

public static class ArcGisIntegrationsEndpoint
{
    private const string TagName = "ArcGisIntegrations";

    public static IEndpointRouteBuilder MapArcGisIntegrations(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.ArcgisIntegrations.FeatureLayerMetadata, async (string serviceName, int layerId, IMediator mediator, CancellationToken token) =>
            {
                var command = new GetLayerMetadataCommand(serviceName, layerId);
                var response = await mediator.Send(command, token);
                var result = new BaseApiResponse<GetLayerMetadataResult>(response);

                return Results.Ok(result);
            })
            .WithName("GetLayerMetadata")
            .WithTags(TagName)
            .WithDescription("Get ArcGIS layer metadata by id")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetLayerMetadataResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.ArcgisIntegrations.AddDataToFeatureLayer, async (UploadFeaturesCommand request, IMediator mediator, CancellationToken token) =>
            {
                var response = await mediator.Send(request, token);
                var result = new BaseApiResponse<UploadFeaturesResult>(response);

                return Results.Ok(result);
            })
            .WithName("AddDataToFeatureLayer")
            .WithTags(TagName)
            .WithDescription("Add Data to the Feature Layer")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UploadFeaturesResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.ArcgisIntegrations.DeleteDataToFeatureLayer, async (DeleteFeaturesCommand command, IMediator mediator, CancellationToken token) =>
            {
                var response = await mediator.Send(command, token);
                var result = new BaseApiResponse<DeleteFeaturesResult>(response);

                return Results.Ok(result);
            })
            .WithName("DeleteDataToFeatureLayer")
            .WithTags(TagName)
            .WithDescription("Delete Data to the Feature Layer")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<DeleteFeaturesResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.ArcgisIntegrations.DeleteAllDataInFeatureLater, async (DeleteAllFeaturesCommand command, IMediator mediator, CancellationToken token) =>
        {
            var response = await mediator.Send(command, token);
            var result = new BaseApiResponse<DeleteAllFeaturesResult>(response);
            return Results.Ok(result);
        }).WithName("DeleteAllDataInFeatureLater")
            .WithTags(TagName)
            .WithDescription("Delete All Data to the Feature Layer")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<DeleteFeaturesResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.ArcgisIntegrations.UpdateDataToFeatureLayer, async (UpdateFeaturesCommand request, IMediator mediator, CancellationToken token) =>
            {
                var response = await mediator.Send(request, token);
                var result = new BaseApiResponse<UpdateFeaturesResult>(response);

                return Results.Ok(result);
            })
            .WithName("UpdateFeatureLayers")
            .WithTags(TagName)
            .WithDescription("Updates ArcGis feature on feature layer or table")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateFeaturesResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);



        app.MapPost(ApiEndpoints.ArcgisIntegrations.QueryDataToFeatureLayer, async (FeatureServiceLayerQuery request, IMediator mediator, CancellationToken token) =>
            {
                var response = await mediator.Send(request, token);
                var result = new BaseApiResponse<FeatureServiceLayerResult>(response);

                return Results.Ok(result);
            })
            .WithName("QueryDataToFeatureLayer")
            .WithTags(TagName)
            .WithDescription("Queries data from ArcGis feature service layer")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<FeatureServiceLayerResult>>()
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}