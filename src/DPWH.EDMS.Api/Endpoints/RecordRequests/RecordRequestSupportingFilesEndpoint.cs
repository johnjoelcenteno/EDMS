using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Models;
using MediatR;
using DPWH.EDMS.Infrastructure.Storage;
using DPWH.EDMS.Application.Features.RecordRequests.Commands.SaveUploadedFile;
using DPWH.EDMS.Application;
using DPWH.EDMS.Application.Features.RecordRequests.Queries;
using DPWH.EDMS.Application.Features.DataLibrary.Queries.GetDataLibraryById;
using DPWH.EDMS.Application.Features.RecordRequests.Commands.SaveRequestedRecordFile;

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
                [AsParameters] UploadRecordRequestFile model,
                IMediator mediator,
                IBlobService blobService,
                CancellationToken token,
                ILogger<Program> logger) =>
            {

                var request = new
                {
                    Id = Guid.NewGuid(),
                    File = model.Document,
                    Filename = model.Document?.FileName
                };

                var metadata = new Dictionary<string, string>();

                byte[] data;
                using (var memoryStream = new MemoryStream())
                {
                    await model.Document.CopyToAsync(memoryStream);
                    data = memoryStream.ToArray();
                }

                metadata.Add("DocumentType", model.DocumentType.ToString());
                metadata.Add("DocumentTypeId", model.DocumentTypeId.ToString());

                //Make sure we are using valid document - ValidIds/AuthorizationDocuments
                var documentType = await mediator.Send(new GetDataLibraryByIdQuery(model.DocumentTypeId));
                if (documentType is null)
                {
                    return Results.BadRequest(new ValidationFailureResponse() { Errors = [new ValidationResponse() { Message = "Invalid DocumentTypeId", PropertyName = "DocumentTypeId" }] });
                }

                var uri = await blobService.Put(WellKnownContainers.RecordRequestSupportingFiles, request.Id.ToString(), data, request.File.ContentType, metadata);

                var command = new SaveUploadedRecordRequestFileCommand(documentType.Value, request.Filename, model.DocumentType.ToString(),
                    model.DocumentTypeId, model.Document.Length, uri);

                var response = await mediator.Send(command, token);

                return TypedResults.Ok(response);
            })
            .WithName("UploadSupportingFile")
            .WithTags(TagName)
            .WithDescription("Upload file and save it as record.")
            .DisableAntiforgery()            
            .Produces<CreateResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);

        app.MapPost(ApiEndpoints.RecordRequest.Documents.UploadRequestedRecordFile, async (
        [AsParameters] UploadRequestedRecordFile model,
        IMediator mediator,
        IBlobService blobService,
        CancellationToken token,
        ILogger<Program> logger) =>
        {
            var request = new
            {
                model.Id,
                File = model.Document,
                Filename = model.Document?.FileName
            };

            var metadata = new Dictionary<string, string>();

            byte[] data;
            using (var memoryStream = new MemoryStream())
            {
                await model.Document.CopyToAsync(memoryStream);
                data = memoryStream.ToArray();
            }

            var uri = await blobService.Put(WellKnownContainers.RequestedRecordFiles, request.Id.ToString(), data, request.File.ContentType, metadata);
            var command = new SaveRequestedRecordFileCommand(model.Id, uri);
            var response = await mediator.Send(command, token);

            return TypedResults.Ok(response);
        })
            .WithName("UploadRequestedRecordFile")
            .WithTags(TagName)
            .WithDescription("Upload requested record file.")
            .DisableAntiforgery()
            .Produces<CreateResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);

        app.MapGet(ApiEndpoints.RecordRequest.Documents.GetSupportingFileById, async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetSupportingFileByIdRequest(id));
            var data = new BaseApiResponse<RecordRequestDocumentModel>(result);

            return result is null ? Results.NotFound() : Results.Ok(data);
        })
        .WithName("GetSupportingFileById")
        .WithTags(TagName)
        .WithDescription("Get supporting file by id")
        .DisableAntiforgery()
        .Produces<BaseApiResponse<RecordRequestDocumentModel>>(StatusCodes.Status200OK)
        .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest); ;

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