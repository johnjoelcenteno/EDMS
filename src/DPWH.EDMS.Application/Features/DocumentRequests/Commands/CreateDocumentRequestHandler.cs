using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;

namespace DPWH.EDMS.Application;

public record class CreateDocumentRequest(Guid employeeRequestId, Guid documentRecordsId, CreateUpdateDocumentRequestModel model) : IRequest<Guid>;
public class CreateDocumentRequestHandler : IRequestHandler<CreateDocumentRequest, Guid>
{
    private readonly IWriteRepository _writeRepository;

    public CreateDocumentRequestHandler(IWriteRepository writeRepository)
    {
        _writeRepository = writeRepository;
    }
    public async Task<Guid> Handle(CreateDocumentRequest request, CancellationToken cancellationToken)
    {
        var documentRequest = CreateUpdateDocumentRequestMapper.MapToEntity(request.model, request.employeeRequestId, request.documentRecordsId);
        _writeRepository.DocumentRequests.Add(documentRequest);
        await _writeRepository.SaveChangesAsync();
        return documentRequest.Id;
    }
}
