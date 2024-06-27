using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.RecordsManagement.Queries.GetRecordsByEmployeeIdQuery;

public record GetRecordsByEmployeeIdQuery(DataSourceRequest Request, string EmployeeId) : IRequest<DataSourceResult>;
public class GetRecordsByEmployeeIdQueryHandler(IReadRepository readRepository) : IRequestHandler<GetRecordsByEmployeeIdQuery, DataSourceResult>
{
    private readonly IReadRepository _readRepository = readRepository;

    public async Task<DataSourceResult> Handle(GetRecordsByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var result = _readRepository.RecordsView
            .Where(p => p.EmployeeId == request.EmployeeId)
            .OrderBy(x => x.RecordName)
            .Select(s => new RecordModel
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                RecordTypeId = s.RecordTypeId,
                RecordName = s.RecordName,
                RecordUri = s.RecordUri
            })
            .ToDataSourceResult(request.Request.FixSerialization());

        return await Task.FromResult(result); ;
    }
}
