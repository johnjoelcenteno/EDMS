using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetRecordTypes;

public record GetRecordTypesQuery : IRequest<IEnumerable<GetRecordTypesResult>>;

internal sealed class GetRecordTypesQueryHandler(IReadRepository repository) : IRequestHandler<GetRecordTypesQuery, IEnumerable<GetRecordTypesResult>>
{
    public async Task<IEnumerable<GetRecordTypesResult>> Handle(GetRecordTypesQuery request, CancellationToken cancellationToken)
    {
        var recordTypes = await repository.DataLibrariesView
            .Where(d => d.Type == WellKnownDataLibraryTypes.RecordTypes)
            .Select(d => new GetRecordTypesResult(d.Id, d.Value))            
            .ToListAsync(cancellationToken);

        return recordTypes.OrderBy(r => r.Name);            
    }
}