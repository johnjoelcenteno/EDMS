using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application;

public record QueryEmployeeRequest(DataSourceRequest dataSourceRequest) : IRequest<DataSourceResult>;

public class QueryEmployeeRecords : IRequestHandler<QueryEmployeeRequest, DataSourceResult>
{
    private readonly IReadRepository _readRepository;

    public QueryEmployeeRecords(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }
    public Task<DataSourceResult> Handle(QueryEmployeeRequest request, CancellationToken cancellationToken)
    {
        var result = _readRepository.EmployeeRecordsView
                    .OrderBy(x => x.Created)
                    .ToDataSourceResult(request.dataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}
