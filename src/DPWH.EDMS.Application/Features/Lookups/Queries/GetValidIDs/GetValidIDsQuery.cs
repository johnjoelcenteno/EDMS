using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetValidIDs;
public record GetValidIDsQuery : IRequest<IEnumerable<GetValidIDsResult>>;

internal sealed class GetValidIDsQueryHandler(IReadRepository repository) : IRequestHandler<GetValidIDsQuery, IEnumerable<GetValidIDsResult>>
{
    public async Task<IEnumerable<GetValidIDsResult>> Handle(GetValidIDsQuery request, CancellationToken cancellationToken)
    {
        var validIDs = await repository.DataLibrariesView
            .Where(d => d.Type == WellKnownDataLibraryTypes.ValidIDs)
            .Select(d => new GetValidIDsResult(d.Id, d.Value))
            .ToListAsync(cancellationToken);

        return validIDs.OrderBy(r => r.Name);
    }
}