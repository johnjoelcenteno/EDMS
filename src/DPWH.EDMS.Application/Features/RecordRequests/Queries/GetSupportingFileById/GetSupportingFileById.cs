using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordRequests.Queries;
using DPWH.EDMS.Domain.Entities;
using MediatR;

namespace DPWH.EDMS.Application;

public record GetSupportingFileByIdRequest(Guid Id) : IRequest<RecordRequestDocumentModel?>;
public class GetSupportingFileById : IRequestHandler<GetSupportingFileByIdRequest, RecordRequestDocumentModel>
{
    private IReadRepository _readRepository;

    public GetSupportingFileById(IReadRepository writeRepository)
    {
        _readRepository = writeRepository;
    }

    public async Task<RecordRequestDocumentModel> Handle(GetSupportingFileByIdRequest request, CancellationToken cancellationToken)
    {
        RecordRequestDocument? recordRequestDocument = _readRepository.RecordRequestDocumentsView.FirstOrDefault(x => x.Id == request.Id);
        if (recordRequestDocument is null)
        {
            return null;
        }

        return RecordRequestDocumentMapper.Map(recordRequestDocument);
    }
}
