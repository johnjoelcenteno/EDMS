using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordTypes.Mappers;
using DPWH.EDMS.Domain;
using MediatR;

namespace DPWH.EDMS.Application.Features.RecordTypes.Queries;

public record class QueryRecordTypeByIdRequest(Guid Id) : IRequest<QueryRecordTypesModel?>;
public class QueryRecordTypeById : IRequestHandler<QueryRecordTypeByIdRequest, QueryRecordTypesModel?>
{
    private readonly IReadRepository _readRepository;

    public QueryRecordTypeById(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<QueryRecordTypesModel?> Handle(QueryRecordTypeByIdRequest request, CancellationToken cancellationToken)
    {
        RecordType? recordType = _readRepository.RecordTypesView.FirstOrDefault(x => x.Id == request.Id);
        if (recordType == null) return null;

        return RecordTypeMappers.Map(recordType);
    }
}
