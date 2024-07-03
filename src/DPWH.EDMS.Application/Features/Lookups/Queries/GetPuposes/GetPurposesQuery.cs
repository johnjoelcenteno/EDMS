using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.DataLibrary;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetPuposes;

public record GetPurposesQuery : IRequest<IEnumerable<GetLookupResult>>;

internal sealed class GetPurposesQueryHandler(IReadRepository repository) : IRequestHandler<GetPurposesQuery, IEnumerable<GetLookupResult>>
{
    public async Task<IEnumerable<GetLookupResult>> Handle(GetPurposesQuery request, CancellationToken cancellationToken)
    {
        var purposes = await repository.DataLibrariesView
         .Where(d => d.Type == DataLibraryTypes.Purposes.ToString())
         .Select(d => new GetLookupResult(d.Id, d.Value))
         .ToListAsync(cancellationToken);

        return purposes.OrderBy(r => r.Name);
    }
}