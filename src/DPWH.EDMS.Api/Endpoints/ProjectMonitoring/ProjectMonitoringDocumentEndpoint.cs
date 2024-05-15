using DPWH.EDMS.Infrastructure.Storage;
using DPWH.EDMS.Api.Endpoints.MaintenanceRequest;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.Maintenance.Commands.DeleteMaintenanceRequestDocument;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.CreateProjectMonitoringBuildingComponentImage;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.CreateProjectMonitoringDocument;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.DeleteProjectMonitoringDocument;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.ProjectMonitoring;

public static class ProjectMonitoringDocumentEndpoint
{
    public static IEndpointRouteBuilder MapProjectMonitoringDocuments(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.ProjectMonitoring.Documents.SaveFile, async (
                [FromRoute] Guid projectMonitoringId,
                Guid? id,
                string? documentName,
                string? group,
                IFormFile? document,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
        {
            var request = new CreateProjectMonitoringDocumentRequest
            {
                Id = string.IsNullOrWhiteSpace(id?.ToString()) || id == Guid.Empty ? Guid.NewGuid() : id,
                ProjectMonitoringId = projectMonitoringId == Guid.Empty ? Guid.NewGuid() : projectMonitoringId,
                Name = documentName,
                Group = group,
                File = document,
                Filename = document?.FileName,
                Category = AssetDocumentCategory.File.ToString()
            };

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

            var uri = await blobService.Put(WellKnownContainers.ProjectMonitoringDocuments, request.Id.ToString(), data, request.File.ContentType, metadata);
            request.Uri = uri;

            var responseWithFile = await mediator.Send(new CreateProjectMonitoringCommand(request));
            return TypedResults.Ok(responseWithFile);
        })
            .WithName("SaveProjectMonitoringFile")
            .WithTags(ProjectMonitoringEndpoint.TagName)
            .WithDescription("Create or update file document.")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);


        app.MapPost(ApiEndpoints.ProjectMonitoring.Documents.SaveImage, async (
                [FromRoute] Guid projectMonitoringBuildingComponentId,
                Guid? id,
                IFormFile? document,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
                    {
                        var request = new CreateProjectMonitoringBuildingComponentImageRequest
                        {
                            Id = string.IsNullOrWhiteSpace(id?.ToString()) || id == Guid.Empty ? Guid.NewGuid() : id,
                            ProjectMonitoringBuildingComponentId = projectMonitoringBuildingComponentId == Guid.Empty ? Guid.NewGuid() : projectMonitoringBuildingComponentId,
                            File = document,
                            Filename = document?.FileName
                        };

                        var metadata = new Dictionary<string, string>();

                        byte[] data;
                        using (var memoryStream = new MemoryStream())
                        {
                            await request.File.CopyToAsync(memoryStream, token);
                            data = memoryStream.ToArray();
                        }

                        request.Uri = await blobService.Put(
                            WellKnownContainers.ProjectMonitoringBuildingComponentImages,
                            request.Id.ToString(),
                            data,
                            request.File.ContentType, metadata);

                        var response = await mediator.Send(new SaveProjectMonitoringBuildingComponentImage(request), token);

                        return Results.Ok(response);
                    })
               .WithName("SaveProjectMonitoringBuildingComponentImage")
               .WithTags(ProjectMonitoringEndpoint.TagName)
               .WithDescription("Save project monitoring building component image.")
               .DisableAntiforgery()
               .Accepts<IFormFile>("multipart/form-data")
               .Produces<SaveProjectMonitoringBuildingComponentImageResponse>(StatusCodes.Status200OK)
               .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
               .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapDelete(ApiEndpoints.ProjectMonitoring.Documents.Delete, async (Guid documentId, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new DeleteProjectMonitoringDocumentCommand(documentId), token);
            return result;
        })
         .WithName("DeleteProjectMonitoringDocument")
         .WithTags(ProjectMonitoringEndpoint.TagName)
         .WithDescription("Delete project monitoring document")
         .WithApiVersionSet(ApiVersioning.VersionSet)
         .HasApiVersion(1.0)
         .Produces<DeleteResponse>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status404NotFound);


        return app;
    }
}
