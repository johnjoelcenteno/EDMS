using DPWH.EDMS.Application.Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionRequestsBuildingComponentsById;
using DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequestBuildingComponentImage;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionRequestById;
using DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequestDocument;
using DPWH.EDMS.Infrastructure.Storage;
using DPWH.EDMS.Api.Endpoints;

namespace DPWH.EDMS.Api.Endpoints.Inspections;

public static class InspectionsImagesEndpoint
{
    public static IEndpointRouteBuilder MapInspectionsImages(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Inspection.SaveImage, async (
                [FromRoute] Guid inspectionRequestBuildingComponentId,
                Guid? id,
                IFormFile? document,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
            {
                //Make sure the inspection request building component exist
                await mediator.Send(new GetInspectionRequestsBuildingComponentsById(inspectionRequestBuildingComponentId), token);

                var request = new SaveInspectionRequestBuildingComponentImageRequest
                {
                    Id = string.IsNullOrWhiteSpace(id.ToString()) ? Guid.NewGuid() : id,
                    InspectionRequestBuildingComponentId = inspectionRequestBuildingComponentId == Guid.Empty ? Guid.NewGuid() : inspectionRequestBuildingComponentId,
                    File = document,
                    Filename = document?.FileName
                };

                logger.LogInformation("Save image {@ImageRequest}", request);

                var metadata = new Dictionary<string, string>();

                byte[] data;
                using (var memoryStream = new MemoryStream())
                {
                    await request.File.CopyToAsync(memoryStream, token);
                    data = memoryStream.ToArray();
                }

                request.Uri = await blobService.Put(
                    WellKnownContainers.InspectionRequestBuildingComponentImages,
                    request.Id.ToString(),
                    data,
                    request.File.ContentType, metadata);

                var response = await mediator.Send(new SaveInspectionRequestBuildingComponentImage(request), token);

                return Results.Ok(response);
            })
           .WithName("SaveInspectionRequestBuildingComponentImage")
           .WithTags(InspectionsEndpoint.TagName)
           .WithDescription("Save inspection request building component image.")
           .DisableAntiforgery()
           .Accepts<IFormFile>("multipart/form-data")
           .Produces<SaveInspectionRequestBuildingComponentImageResponse>(StatusCodes.Status200OK)
           .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.Inspection.SaveFile, async (
                [FromRoute] Guid inspectionRequestId,
                Guid? id,
                string? name,
                IFormFile? document,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
        {
            //Make sure rental rate exist
            var inspectionRequest = await mediator.Send(new GetInspectionRequestByIdCommand(inspectionRequestId), token);
            var request = new CreateInspectionRequestDocumentRequest
            {
                Id = string.IsNullOrWhiteSpace(id.ToString()) ? Guid.NewGuid() : id,
                InspectionRequestId = inspectionRequest.Id,
                Name = name,
                File = document,
                Filename = document?.FileName,
            };

            logger.LogInformation("Save document file {@AssetFileRequest}", request);

            byte[]? data = null;
            if (request.File != null && request.File.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await request.File.CopyToAsync(memoryStream, token);
                    data = memoryStream.ToArray();
                }
            }

            var metadata = new Dictionary<string, string> { { "Category", AssetDocumentCategory.File.ToString() } };
            request.Uri = await blobService.Put(
                WellKnownContainers.InspectionRequestDocuments,
                request.Id.ToString(),
                data,
                request.File.ContentType,
                metadata);

            var response = await mediator.Send(new CreateInspectionRequestDocumentCommand(request), token);
            return Results.Ok(response);
        })
            .WithName("SaveInspectionRequestFile")
            .WithTags(InspectionsEndpoint.TagName)
            .WithDescription("Create or update file document.")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<SaveInspectionRequestDocumentResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        return app;
    }
}
