using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Infrastructure.Storage;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesDocument;
using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesDocument.File;
using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesDocument.Image;
using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesPropertyDocument;
using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.DeleteRentalRatesDocument;
using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesById;
using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesPropertyById;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Inspections;

public static class RentalRatesDocumentEndpoint
{
    public static IEndpointRouteBuilder MapRentalRatesDocument(this IEndpointRouteBuilder app)
    {

        app.MapPost(ApiEndpoints.RentalRates.Documents.SaveImage, async (
                [FromRoute] Guid rentalRatesId,
                Guid? id,
                string? group,
                IFormFile? document,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
            {

                //Make sure the rental rate exist
                var rentalRate = await mediator.Send(new GetRentalRatesByIdQuery(rentalRatesId), token);
                var request = new CreateRentalRatesDocumentRequest
                {
                    Id = string.IsNullOrWhiteSpace(id.ToString()) ? Guid.NewGuid() : id,
                    RentalRatesId = rentalRate.Id,
                    File = document,
                    Filename = document?.FileName,
                    Group = group
                };

                logger.LogInformation("Save image {@ImageRequest}", request);


                byte[] data;
                using (var memoryStream = new MemoryStream())
                {
                    await request.File.CopyToAsync(memoryStream, token);
                    data = memoryStream.ToArray();
                }

                var metadata = new Dictionary<string, string> { { "Category", AssetDocumentCategory.Image.ToString() } };
                request.Uri = await blobService.Put(
                    WellKnownContainers.RentalRateDocuments,
                    request.Id.ToString(),
                    data,
                    request.File.ContentType,
                    metadata);

                var response = await mediator.Send(new CreateRentalRatesImageCommand(request), token);

                return Results.Ok(response);
            })
           .WithName("SaveRentalRatesImage")
           .WithTags(RentalRatesEndpoint.TagName)
           .WithDescription("Save inspection request building component image.")
           .DisableAntiforgery()
           .Accepts<IFormFile>("multipart/form-data")
           .Produces<SaveRentalRateDocumentResponse>(StatusCodes.Status200OK)
           .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.RentalRates.Documents.SaveFile, async (
                [FromRoute] Guid rentalRatesId,
                Guid? id,
                string? name,
                IFormFile? document,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
            {
                //Make sure rental rate exist
                var rentalRate = await mediator.Send(new GetRentalRatesByIdQuery(rentalRatesId), token);
                var request = new CreateRentalRatesDocumentRequest
                {
                    Id = string.IsNullOrWhiteSpace(id.ToString()) ? Guid.NewGuid() : id,
                    RentalRatesId = rentalRate.Id,
                    Name = name,
                    File = document,
                    Filename = document?.FileName,
                    Category = AssetDocumentCategory.File.ToString()
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
                    WellKnownContainers.RentalRateDocuments,
                    request.Id.ToString(),
                    data,
                    request.File.ContentType,
                    metadata);

                var response = await mediator.Send(new CreateRentalRateFileCommand(request), token);
                return Results.Ok(response);
            })
            .WithName("SaveRentalRatesFile")
            .WithTags(RentalRatesEndpoint.TagName)
            .WithDescription("Create or update file document.")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<SaveRentalRateDocumentResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        app.MapPost(ApiEndpoints.RentalRates.Property.Documents.SaveFile, async (
            [FromRoute] Guid rentalRatesPropertyId,
            Guid? id,
            string? name,
            IFormFile? document,
            IMediator mediator,
            IBlobService blobService,
            CancellationToken token,
            ILogger<Program> logger) =>
        {
            //Make sure rental rate property exist
            var rentalRate = await mediator.Send(new GetRentalRatesPropertyByIdQuery(rentalRatesPropertyId), token) ??
                             throw new AppException("Rental rate property not found.");

            var request = new CreateRentalRatesPropertyDocumentRequest
            {
                Id = string.IsNullOrWhiteSpace(id.ToString()) ? Guid.NewGuid() : id,
                RentalRatesPropertyId = rentalRate.Id,
                Name = name,
                File = document,
                Filename = document?.FileName,
                Category = AssetDocumentCategory.File.ToString()
            };

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
                WellKnownContainers.RentalRatePropertyDocuments,
                request.Id.ToString(),
                data,
                request.File.ContentType,
                metadata);

            var response = await mediator.Send(new CreateRentalRatesPropertyDocumentCommand(request), token);
            return Results.Ok(response);
        })
            .WithName("SaveRentalRatesPropertyFile")
            .WithTags(RentalRatesEndpoint.TagName)
            .WithDescription("Create or update file document.")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<SaveRentalRateDocumentResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);



        app.MapDelete(ApiEndpoints.RentalRates.Documents.Delete, async (
                Guid documentId,
                IMediator mediator,
                CancellationToken token) =>
            {
                var result = await mediator.Send(new DeleteDocument(documentId), token);
                return Results.Ok(result);
            })
            .WithName("DeleteRentalRateDocument")
            .WithTags(RentalRatesEndpoint.TagName)
            .WithDescription("Delete rental rates document")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DeleteResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError); ;

        return app;
    }
}
