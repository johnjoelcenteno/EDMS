using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;
using System.Security.Claims;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequestDocument;

public record CreateInspectionRequestDocumentCommand(CreateInspectionRequestDocumentRequest Request) : IRequest<SaveInspectionRequestDocumentResponse>;
public class CreateInspectionRequestDocumentHandler : IRequestHandler<CreateInspectionRequestDocumentCommand, SaveInspectionRequestDocumentResponse>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public CreateInspectionRequestDocumentHandler(IWriteRepository writeRepository, ClaimsPrincipal principal)
    {
        _repository = writeRepository;
        _principal = principal;
    }

    public async Task<SaveInspectionRequestDocumentResponse> Handle(CreateInspectionRequestDocumentCommand request, CancellationToken cancellationToken)
    {
        var model = request.Request;
        long fileSize = model.File.Length;

        var rentalRate = _repository.InspectionRequests.FirstOrDefault(i => i.Id == model.InspectionRequestId);
        var document = _repository.InspectionRequestDocuments.FirstOrDefault(i => i.Id == model.Id);

        if (document is null)
        {
            document = InspectionRequestDocument.Create(model.Id ?? Guid.NewGuid(), rentalRate.Id, model.Name, model.Filename, fileSize, model.Uri, _principal.GetUserName());
            _repository.InspectionRequestDocuments.Add(document);
        }
        else
        {
            document.Update(model.Name, model.Filename, fileSize, model.Uri, _principal.GetUserName());
        }
        await _repository.SaveChangesAsync(cancellationToken);

        return new SaveInspectionRequestDocumentResponse(document.Id);
    }
}
