using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Infrastructure.Storage;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.Maintenance.Commands.CreateMaintenanceRequestDocument;
using DPWH.EDMS.Application.Features.Maintenance.Commands.DeleteMaintenanceRequestDocument;
using DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequestById;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.MaintenanceRequest;

public static class MaintenanceRequestDocumentEndpoint
{
    public static IEndpointRouteBuilder MapMaintenanceRequestDocuments(this IEndpointRouteBuilder app)
    {

        app.MapPost(ApiEndpoints.Maintenance.Documents.SaveFile, async (
                [FromRoute] Guid maintenanceRequestId,
                Guid? id,
                string? documentName,
                string? group,
                IFormFile? document,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
            {
                var request = new CreateMaintenanceRequestDocumentRequest
                {
                    Id = string.IsNullOrWhiteSpace(id.ToString()) ? Guid.NewGuid() : id,
                    MaintenanceRequestId = maintenanceRequestId == Guid.Empty ? Guid.NewGuid() : maintenanceRequestId,
                    Name = documentName,
                    Group = EnumExtensions.GetDescriptionFromValue<MaintenanceDocumentType>(group),
                    File = document,
                    Filename = document?.FileName,
                    Category = AssetDocumentCategory.File.ToString()
                };

                logger.LogInformation("Save document file {@AssetFileRequest}", request);

                //Make sure rentalrate exist
                var maintenanceRequest = await mediator.Send(new GetMaintenanceRequestByIdQuery(maintenanceRequestId), token);

                if (maintenanceRequest is null)
                {
                    logger.LogWarning("The requested rental rate does not exist {RequestId}", maintenanceRequestId);
                    return Results.BadRequest("Maintenance Request not found");
                }

                IDictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("Category", AssetDocumentCategory.File.ToString());

                byte[]? data = null;

                if (request.File != null && request.File.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await request.File.CopyToAsync(memoryStream);
                        data = memoryStream.ToArray();
                    }
                }

                var uri = await blobService.Put(WellKnownContainers.MaintenanceRequestDocuments, request.Id.ToString(), data, request.File.ContentType, metadata);
                request.Uri = uri;

                var responseWithFile = await mediator.Send(new CreateMaintenanceRequestDocumentCommand(request));
                return TypedResults.Ok(responseWithFile);
            })
            .WithName("SaveMaintenanceRequestFile")
            .WithTags(MaintenanceRequestEndpoint.TagName)
            .WithDescription("Create or update file document.")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<SaveMaintenanceRequestDocumentResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);

        app.MapDelete(ApiEndpoints.Maintenance.Documents.Delete, async (Guid documentId, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new DeleteMaintenanceRequestDocumentCommand(documentId), token);
            return result;
        })
         .WithName("DeleteMaintenanceRequestDocument")
         .WithTags(MaintenanceRequestEndpoint.TagName)
         .WithDescription("Delete rental rates document")
         .WithApiVersionSet(ApiVersioning.VersionSet)
         .HasApiVersion(1.0)
         .Produces<DeleteResponse>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
