using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetEmployeeDocuments;

public record GetEmployeeDocumentsQuery : IRequest<IEnumerable<GetLookupResult>>;

internal sealed class GetEmployeeDocumentsQueryHandler(IReadRepository repository) : IRequestHandler<GetEmployeeDocumentsQuery, IEnumerable<GetLookupResult>>
{
    public async Task<IEnumerable<GetLookupResult>> Handle(GetEmployeeDocumentsQuery request, CancellationToken cancellationToken)
    {
        var category = EnumExtensions.GetDescriptionFromValue<RecordTypesCategory>(RecordTypesCategory.EmployeeDocuments.ToString());
        var recordTypes = await repository.RecordTypesView
            .Where(d => d.Category == category)
            .Select(d => new GetLookupResult(d.Id, d.Name))
            .ToListAsync(cancellationToken);

        return recordTypes.OrderBy(r => r.Name);
    }
}