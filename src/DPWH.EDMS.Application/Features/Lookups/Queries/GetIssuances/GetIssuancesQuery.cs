using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordTypes;
using DPWH.EDMS.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetIssuances;

public record GetIssuancesQuery : IRequest<IEnumerable<GetLookupResult>>;

internal sealed class GetIssuancesQueryHandler(IReadRepository repository) : IRequestHandler<GetIssuancesQuery, IEnumerable<GetLookupResult>>
{
    public async Task<IEnumerable<GetLookupResult>> Handle(GetIssuancesQuery request, CancellationToken cancellationToken)
    {
        var category = EnumExtensions.GetDescriptionFromValue<RecordTypesCategory>(RecordTypesCategory.Issuances.ToString());
        var recordTypes = await repository.RecordTypesView
            .Where(d => d.Category == category)
            .Select(d => new GetLookupResult(d.Id, d.Name))
            .ToListAsync(cancellationToken);

        return recordTypes.OrderBy(r => r.Name);
    }
}