using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.RecordsManagement.Queries.GetRecordsQuery;

public record GetRecordsQuery(DataSourceRequest Request) : IRequest<DataSourceResult>;
public class GetRecordsQueryHandler(IReadRepository readRepository) : IRequestHandler<GetRecordsQuery, DataSourceResult>
{
    private readonly IReadRepository _readRepository = readRepository;

    public async Task<DataSourceResult> Handle(GetRecordsQuery request, CancellationToken cancellationToken)
    {
        var result = _readRepository.RecordsView
            .OrderBy(x => x.RecordName)
            .Select(s => new RecordModel
            {
                Id = s.Id,
                EmployeeId = s.EmployeeId,
                RecordTypeId = s.RecordTypeId,
                RecordName = s.RecordName,
                RecordUri = s.RecordUri,
                DocVersion = s.Created.ToString("MMddyyyy")
            })
            .ToDataSourceResult(request.Request.FixSerialization());

        return await Task.FromResult(result); ;
    }
}
