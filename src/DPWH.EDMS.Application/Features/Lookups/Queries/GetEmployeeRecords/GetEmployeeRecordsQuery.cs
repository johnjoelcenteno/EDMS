using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.RecordTypes;
using DPWH.EDMS.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetEmployeeRecords;

public record GetEmployeeRecordsQuery : IRequest<IEnumerable<GetLookupResult>>;

internal sealed class GetEmployeeRecordsQueryHandler(IReadRepository repository) : IRequestHandler<GetEmployeeRecordsQuery, IEnumerable<GetLookupResult>>
{
    public async Task<IEnumerable<GetLookupResult>> Handle(GetEmployeeRecordsQuery request, CancellationToken cancellationToken)
    {
        var category = EnumExtensions.GetDescriptionFromValue<RecordTypesCategory>(RecordTypesCategory.EmployeeRecords.ToString());
        var recordTypes = await repository.RecordTypesView
            .Where(d => d.Category == category)
            .Select(d => new GetLookupResult(d.Id, d.Name))
            .ToListAsync(cancellationToken);

        return recordTypes.OrderBy(r => r.Name);
    }
}