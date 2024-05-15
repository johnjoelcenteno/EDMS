using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.Assets.Queries;
using DPWH.EDMS.Application.Models;
using MediatR;
using DPWH.EDMS.Application.Features.Assets.Commands;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssetById;
using DPWH.EDMS.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using DPWH.EDMS.Infrastructure.Storage;

namespace DPWH.EDMS.Api.Endpoints.Assets;

public static class AssetDocumentsEndpoint
{
    private const string TagName = "Assets";

    public static IEndpointRouteBuilder MapAssetDocuments(this IEndpointRouteBuilder app)
    {
        #region "GETs"        

        app.MapGet(ApiEndpoints.Assets.AssetDocuments.GetImages, async ([FromRoute] Guid assetId, IMediator mediator, CancellationToken token) =>
            {
                var asset = await mediator.Send(new GetAssetByIdQuery(assetId), token);
                var assetDocuments = await mediator.Send(new GetAssetImages(asset.Model.Id), token);

                return Results.Ok(assetDocuments);
            })
            .WithName("GetAssetImages")
            .WithTags(TagName)
            .WithDescription("Get asset images using the assetId.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<IEnumerable<AssetImageModel>>(StatusCodes.Status200OK);

        app.MapGet(ApiEndpoints.Assets.AssetDocuments.GetFiles, async ([FromRoute] Guid assetId, IMediator mediator, CancellationToken token) =>
            {
                var asset = await mediator.Send(new GetAssetByIdQuery(assetId), token);
                var assetDocuments = await mediator.Send(new GetAssetFiles(asset.Model.Id), token);

                return Results.Ok(assetDocuments);
            })
            .WithName("GetAssetFiles")
            .WithTags(TagName)
            .WithDescription("Get asset files using the assetId.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<IEnumerable<AssetFileModel>>(StatusCodes.Status200OK);

        #endregion

        #region "POSTs"

        app.MapPost(ApiEndpoints.Assets.AssetDocuments.SaveImage, async (
                [FromRoute] Guid assetId,
                string id,
                AssetImageView view,
                IFormFile? document,
                double? longDegrees,
                double? longMinutes,
                double? longSeconds,
                string? longDirection,
                double? latDegrees,
                double? latMinutes,
                double? latSeconds,
                string? latDirection,
                string? description,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
            {
                var documentId = Guid.Parse(id);
                //Make sure asset exist
                var asset = await mediator.Send(new GetAssetByIdQuery(assetId), token);

                var isCreate = documentId == Guid.Empty;
                var request = new SaveAssetImageRequest
                {
                    Id = isCreate ? Guid.NewGuid() : documentId,
                    AssetId = asset.Model.Id,
                    File = document,
                    Filename = document?.FileName,
                    View = view,
                    Description = description,
                    LongDegrees = longDegrees,
                    LongMinutes = longMinutes,
                    LongSeconds = longSeconds,
                    LongDirection = longDirection,
                    LatDegrees = latDegrees,
                    LatMinutes = latMinutes,
                    LatSeconds = latSeconds,
                    LatDirection = latDirection,
                };

                logger.LogInformation("Save image {@ImageRequest}", request);

                if (!isCreate)
                {
                    //check if image exist
                    var assetImage = await mediator.Send(new GetAssetImage(documentId), token);
                    if (assetImage is null)
                    {
                        logger.LogWarning("The requested asset image does not exist {RequestId}", request.Id);
                        return Results.NotFound("Image asset not found");
                    }
                }

                var metadata = new Dictionary<string, string>();

                byte[] data;
                using (var memoryStream = new MemoryStream())
                {
                    await request.File.CopyToAsync(memoryStream);
                    data = memoryStream.ToArray();
                }

                metadata.Add("Category", AssetDocumentCategory.Image.ToString());
                metadata.Add("DocumentType", AssetDocumentCategory.Image.ToString());

                var uri = await blobService.Put(WellKnownContainers.AssetDocuments, request.Id.ToString(), data, request.File.ContentType, metadata);
                request.Uri = uri;

                var response = await mediator.Send(new SaveAssetImage(request), token);

                return TypedResults.Ok(response);
            })
            .WithName("SaveAssetImage")
            .WithTags(TagName)
            .WithDescription("Create or update image document.")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<SaveAssetImageResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);

        app.MapPost(ApiEndpoints.Assets.AssetDocuments.SaveFile, async (
                [FromRoute] Guid assetId,
                string id,
                [FromRoute] string documentType,
                IFormFile? document,
                string? documentNo,
                string? documentTypeOthers,
                string? otherRelatedDocuments,
                string? description,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
            {
                var documentId = Guid.Parse(id);
                //Make sure asset exist
                var asset = await mediator.Send(new GetAssetByIdQuery(assetId), token);

                var isCreate = documentId == Guid.Empty;
                var request = new SaveAssetFileRequest
                {
                    Id = isCreate ? Guid.NewGuid() : documentId,
                    AssetId = asset.Model.Id,
                    File = document,
                    DocumentType = documentType,
                    Filename = document?.FileName,
                    DocumentNo = documentNo,
                    DocumentTypeOthers = documentTypeOthers ?? "N/A",
                    OtherRelatedDocuments = otherRelatedDocuments ?? "N/A",
                    Description = description
                };

                logger.LogInformation("Save document file {@AssetFileRequest}", request);

                IDictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("Category", AssetDocumentCategory.File.ToString());
                metadata.Add("DocumentType", documentType);

                if (!isCreate && request.File == null)
                {
                    //check if file asset exist
                    var assetFile = await mediator.Send(new GetAssetFile(documentId), token);

                    if (assetFile is null)
                    {
                        logger.LogWarning("The requested asset file does not exist {RequestId}", request.Id);
                        return Results.NotFound("File asset not found");
                    }
                    var response = await mediator.Send(new SaveAssetFile(request));
                    return TypedResults.Ok(response);
                }

                byte[]? data = null;

                if (request.File != null && request.File.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await request.File.CopyToAsync(memoryStream);
                        data = memoryStream.ToArray();
                    }
                }

                var uri = await blobService.Put(WellKnownContainers.AssetDocuments, request.Id.ToString(), data, request.File!.ContentType, metadata);
                request.Uri = uri;
                var responseWithFile = await mediator.Send(new SaveAssetFile(request), token);
                return Results.Ok(responseWithFile);
            })
            .WithName("SaveAssetFile")
            .WithTags(TagName)
            .WithDescription("Create or update file document.")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<SaveAssetFileResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);

        app.MapPost(ApiEndpoints.Assets.AssetDocuments.UpdateFileProperties, async (
                [FromRoute] Guid assetId,
                string id,
                [FromRoute] string documentType,
                string? documentNo,
                string? documentTypeOthers,
                string? otherRelatedDocuments,
                string? description,
                IMediator mediator,
                CancellationToken token,
                ILogger<Program> logger) =>
            {
                var documentId = Guid.Parse(id);
                //Make sure asset exist
                var asset = await mediator.Send(new GetAssetByIdQuery(assetId), token);

                var isCreate = documentId == Guid.Empty;
                var request = new SaveAssetFileRequest
                {
                    Id = isCreate ? Guid.NewGuid() : documentId,
                    AssetId = asset.Model.Id,
                    DocumentType = documentType,
                    DocumentNo = documentNo,
                    DocumentTypeOthers = documentTypeOthers ?? "N/A",
                    OtherRelatedDocuments = otherRelatedDocuments ?? "N/A",
                    Description = description
                };

                logger.LogInformation("Save document file properties {@AssetFileRequest}", request);

                if (!isCreate)
                {
                    //check if file asset exist
                    var assetFile = await mediator.Send(new GetAssetFile(documentId), token);

                    if (assetFile is null)
                    {
                        logger.LogWarning("The requested asset file does not exist {RequestId}", request.Id);
                        return Results.NotFound("File asset not found");
                    }
                }

                IDictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("Category", AssetDocumentCategory.File.ToString());
                metadata.Add("DocumentType", documentType);

                var response = await mediator.Send(new SaveAssetFile(request));
                return Results.Ok(response);
            })
            .WithName("UpdateFileProperties")
            .WithTags(TagName)
            .WithDescription("Create or update file document properties.")
            .Produces<SaveAssetFileResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);

        #region Financial Details Funding History Documents
        app.MapPost(ApiEndpoints.Assets.AssetDocuments.SaveFinancialFile, async (
                [FromRoute] Guid assetId,
                string id,
                IFormFile? document,
                string? yearFunded,
                double? allocation,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token) =>
            {
                var documentId = Guid.Parse(id);
                var asset = await mediator.Send(new GetAssetByIdQuery(assetId), token);
                var request = new SaveFinancialFileRequest
                {
                    Id = documentId == Guid.Empty ? Guid.NewGuid() : documentId,
                    AssetId = asset.Model.Id,
                    YearFunded = yearFunded,
                    Allocation = allocation,
                    FileName = document?.FileName,
                    File = document
                };

                if (request.File == null)
                {
                    var response = await mediator.Send(new SaveFinancialFile(request), token);
                    return TypedResults.Ok(response);
                }

                byte[]? data = null;

                if (request.File != null && request.File.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await request.File.CopyToAsync(memoryStream);
                        data = memoryStream.ToArray();
                    }
                }

                var uri = await blobService.Put(WellKnownContainers.FinancialDocuments, request.FileName, data, request.File!.ContentType);
                request.Uri = uri;
                var responseWithFile = await mediator.Send(new SaveFinancialFile(request));
                return Results.Ok(responseWithFile);
            })
            .WithName("SaveFinancialFile")
            .WithTags(TagName)
            .WithDescription("Create or update file document.")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<SaveFinancialFileResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);


        app.MapPost(ApiEndpoints.Assets.AssetDocuments.UpdateFinancialFileProperties, async (
                [FromRoute] Guid assetId,
                string id,
                string? yearFunded,
                double? allocation,
                IMediator mediator,
                CancellationToken token) =>
            {
                var documentId = Guid.Parse(id);
                var asset = await mediator.Send(new GetAssetByIdQuery(assetId), token);
                var request = new SaveFinancialFileRequest
                {
                    Id = documentId == Guid.Empty ? Guid.NewGuid() : documentId,
                    AssetId = asset.Model.Id,
                    YearFunded = yearFunded,
                    Allocation = allocation
                };

                var response = await mediator.Send(new SaveFinancialFile(request), token);
                return Results.Ok(response);
            })
            .WithName("UpdateFinancialFile")
            .WithTags(TagName)
            .WithDescription("Update file document properties.")
            .Produces<SaveFinancialFileResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);

        #endregion

        app.MapDelete(ApiEndpoints.Assets.AssetDocuments.DeleteDocument, async (
                Guid assetId,
                string id,
                IMediator mediator,
                CancellationToken token) =>
            {
                _ = await mediator.Send(new GetAssetByIdQuery(assetId), token);

                var documentId = Guid.Parse(id);
                var result = await mediator.Send(new DeleteWithId<DeleteAssetDocumentRequest, DeleteResponse>(documentId), token);
                return result;
            })
            .WithName("DeleteAssetDocument")
            .WithTags(TagName)
            .WithDescription("Delete asset document")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DeleteResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        #endregion

        return app;
    }
}