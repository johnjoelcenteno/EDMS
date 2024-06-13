using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.Assets.Queries;
using DPWH.EDMS.Application.Models;
using MediatR;
using DPWH.EDMS.Application.Features.Assets.Commands;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssetById;
using DPWH.EDMS.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using DPWH.EDMS.Infrastructure.Storage;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Application.Features.RecordRequests.Commands.SaveUploadedFile;

namespace DPWH.EDMS.Api.Endpoints.RecordRequests;

public static class RecordRequestSupportingFilesEndpoint
{
    private const string TagName = "RecordRequestSupportingFiles";

    public static IEndpointRouteBuilder MapRecordRequestSupportingFiles(this IEndpointRouteBuilder app)
    {
        #region "GETs"        

        #endregion

        #region "POSTs"

        app.MapPost(ApiEndpoints.RecordRequest.Documents.UploadSupportingFile, async (                
                RecordRequestProvidedDocumentTypes documentType,
                IFormFile? document,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
            {
                
                var request = new 
                {
                    Id = Guid.NewGuid(),                    
                    File = document,
                    Filename = document?.FileName                    
                };

                var metadata = new Dictionary<string, string>();

                byte[] data;
                using (var memoryStream = new MemoryStream())
                {
                    await document.CopyToAsync(memoryStream);
                    data = memoryStream.ToArray();
                }

                metadata.Add("DocumentType", documentType.ToString());

                var uri = await blobService.Put(WellKnownContainers.RecordRequestSupportingFiles, request.Id.ToString(), data, request.File.ContentType, metadata);

                var command = new SaveUploadedRecordRequestFileCommand(request.Filename, request.Filename, documentType.ToString(), document.Length, uri);

                var response = await mediator.Send(command, token);

                return TypedResults.Ok(response);
            })
            .WithName("UploadSupportingFile")
            .WithTags(TagName)
            .WithDescription("Upload file and save it as record.")
            .DisableAntiforgery()
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<CreateResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);


        //app.MapDelete(ApiEndpoints.Assets.AssetDocuments.DeleteDocument, async (
        //        Guid assetId,
        //        string id,
        //        IMediator mediator,
        //        CancellationToken token) =>
        //    {
        //        _ = await mediator.Send(new GetAssetByIdQuery(assetId), token);

        //        var documentId = Guid.Parse(id);
        //        var result = await mediator.Send(new DeleteWithId<DeleteAssetDocumentRequest, DeleteResponse>(documentId), token);
        //        return result;
        //    })
        //    .WithName("DeleteAssetDocument")
        //    .WithTags(TagName)
        //    .WithDescription("Delete asset document")
        //    .WithApiVersionSet(ApiVersioning.VersionSet)
        //    .HasApiVersion(1.0)
        //    .Produces<DeleteResponse>(StatusCodes.Status200OK)
        //    .Produces(StatusCodes.Status404NotFound);

        #endregion

        return app;
    }
}