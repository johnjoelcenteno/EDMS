using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetSecondaryIDs;
public record GetSecondaryIDsQuery : IRequest<IEnumerable<GetSecondaryIDsResult>>;

internal sealed class GetSecondaryIDsQueryHandler(IReadRepository repository) : IRequestHandler<GetSecondaryIDsQuery, IEnumerable<GetSecondaryIDsResult>>
{
    public async Task<IEnumerable<GetSecondaryIDsResult>> Handle(GetSecondaryIDsQuery request, CancellationToken cancellationToken)
    {
        var recordTypes = await repository.DataLibrariesView
            .Where(d => d.Type == WellKnownDataLibraryTypes.SecondaryIDs)
            .Select(d => new GetSecondaryIDsResult(d.Id, d.Value))
            .ToListAsync(cancellationToken);

        return recordTypes.OrderBy(r => r.Name);
    }
}