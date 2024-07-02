using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordTypes.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.RecordTypes.Queries;

public record class QueryRecordTypesByCategoryRequest(string category) : IRequest<List<QueryRecordTypesModel>?>;
public class QueryRecordTypesByCategory : IRequestHandler<QueryRecordTypesByCategoryRequest, List<QueryRecordTypesModel>?>
{
    private IReadRepository _readRepository;

    public QueryRecordTypesByCategory(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }
    public Task<List<QueryRecordTypesModel>> Handle(QueryRecordTypesByCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = _readRepository.RecordTypesView
                    .Where(x => x.Category == request.category)
                    .Select(x => RecordTypeMappers.Map(x))
                    .ToListAsync();
        return result;
    }
}
