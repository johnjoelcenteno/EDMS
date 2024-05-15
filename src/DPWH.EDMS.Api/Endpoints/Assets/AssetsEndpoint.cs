using DPWH.EDMS.Application.Features.Assets.Commands.CreateAsset;
using DPWH.EDMS.Application.Features.Assets.Commands.PatchAsset;
using DPWH.EDMS.Application.Features.Assets.Commands.UpdateAsset;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssetsByBuildingId;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssetsByImplementingOffice;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssetsByRegionalOffice;
using DPWH.EDMS.Application.Features.Assets.Commands.ValidateAsset;
using DPWH.EDMS.Application.Features.Assets.Models;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssetById;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssets;

namespace DPWH.EDMS.Api.Endpoints.Assets;

public static class AssetsEndpoint
{
    private const string TagName = "Assets";

    public static IEndpointRouteBuilder MapAssets(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Assets.Create, async (CreateAssetRequest request, IMediator mediator, CancellationToken token, ILogger<CreateAssetRequest> logger) =>
            {
                logger.LogInformation("Creating new asset {@CreateAssetRequest}", request);

                var response = await mediator.Send(new CreateAssetCommand(request), token);

                return TypedResults.Ok(response);
            })
            .WithName("CreateAsset")
            .WithTags(TagName)
            .WithDescription("Create new asset.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<CreateResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.Assets.Get, async ([FromRoute] Guid id, IMediator mediator, CancellationToken token) =>
            {
                var asset = await mediator.Send(new GetAssetByIdQuery(id), token);

                return Results.Ok(asset);
            })
            .WithName("GetAsset")
            .WithTags(TagName)
            .WithDescription("Get asset using the id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<AssetResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .AllowAnonymous();

        app.MapPost(ApiEndpoints.Assets.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAssetsQuery(request), token);

                return result;
            })
            .WithName("QueryAsset")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.Assets.GetByImplementingOffice, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAssetsByImplementingOffice(request), token);

                return result;
            })
            .WithName("GetAssetsByImplementingOffice")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.Assets.GetByRegionalOffice, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAssetsByRegionalOffice(request), token);

                return result;
            })
            .WithName("GetAssetsByRegionalOffice")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.Assets.Update, async (UpdateAssetRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new UpdateAssetCommand(request), token);
                var data = new BaseApiResponse<UpdateAssetResult>(result);

                return Results.Ok(data);
            })
            .WithName("UpdateAsset")
            .WithTags(TagName)
            .WithDescription("Update asset")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateAssetResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.Assets.Validate, async (ValidateAssetRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new ValidateAssetCommand(request), token);
                var data = new BaseApiResponse<ValidateAssetResult>(result);

                return Results.Ok(data);
            })
             .WithName("PropertyEntryValidation")
             .WithTags(TagName)
             .WithDescription("Validate asset by inspector")
             .WithApiVersionSet(ApiVersioning.VersionSet)
             .HasApiVersion(1.0)
             .Produces<BaseApiResponse<ValidateAssetResult>>()
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
             .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.Assets.UpdateStatus, async ([FromRoute] Guid id, UpdateAssetStatusRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new UpdateWithId<UpdateAssetStatusRequest, UpdateResponse>(id, request), token);

                return result;
            })
            .WithName("UpdateAssetStatus")
            .WithTags(TagName)
            .WithDescription("Update asset status. E.g from Active to Archived")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<UpdateResponse>()
            .Produces(StatusCodes.Status404NotFound);

        app.MapPatch(ApiEndpoints.Assets.Patch, async ([FromRoute] Guid id, PatchAssetRequest patchAsset, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new PatchAssetCommand(id, patchAsset), token);
                return result;
            })
            .WithName("PatchUpdateAsset")
            .WithTags(TagName)
            .WithDescription("Partial update of asset")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<Guid>()
            .Produces(StatusCodes.Status404NotFound);

        app.MapGet(ApiEndpoints.Assets.GetByBuildingId, async (string id, string? purpose, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAssetsByBuildingId(id, purpose), token);
                var data = new BaseApiResponse<GetAssetsByBuildingIdResult>(result);

                return data.Data is null ? Results.NotFound() : Results.Ok(data);
            })
            .WithName("GetAssetsByBuildingId")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetAssetsByBuildingIdResult>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}