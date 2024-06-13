using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.SaveUploadedFile;

public record class SaveUploadedRecordRequestFileCommand(string Name, string Filename, string Category,
    long FileSize, string Uri): IRequest<CreateResponse>;
internal sealed class UploadRecordRequestFileCommandHandler(IWriteRepository writeRepository, ClaimsPrincipal principal) : IRequestHandler<SaveUploadedRecordRequestFileCommand, CreateResponse>
{
    public async Task<CreateResponse> Handle(SaveUploadedRecordRequestFileCommand request, CancellationToken cancellationToken)
    {
        var model = request;

        var recordRequestDocument = RecordRequestDocument.Create(model.Name, model.Filename, model.Category, model.FileSize,
            model.Uri, principal.GetUserName());

        writeRepository.RecordRequestDocuments.Add(recordRequestDocument);
        await writeRepository.SaveChangesAsync(cancellationToken);

        return new CreateResponse(recordRequestDocument.Id);
    }
}
