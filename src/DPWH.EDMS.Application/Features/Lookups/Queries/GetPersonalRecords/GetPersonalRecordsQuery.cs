using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetPersonalRecords;

public record GetPersonalRecordsQuery : IRequest<IEnumerable<GetLookupResult>>;

internal sealed class GetPersonalRecordsQueryHandler(IReadRepository repository) : IRequestHandler<GetPersonalRecordsQuery, IEnumerable<GetLookupResult>>
{
    public async Task<IEnumerable<GetLookupResult>> Handle(GetPersonalRecordsQuery request, CancellationToken cancellationToken)
    {
        var category = EnumExtensions.GetDescriptionFromValue<RecordTypesCategory>(RecordTypesCategory.PersonalRecords.ToString());
        var recordTypes = await repository.RecordTypesView
            .Where(d => d.Category == category)
            .Select(d => new GetLookupResult(d.Id, d.Name))
            .ToListAsync(cancellationToken);

        return recordTypes.OrderBy(r => r.Name);
    }
}