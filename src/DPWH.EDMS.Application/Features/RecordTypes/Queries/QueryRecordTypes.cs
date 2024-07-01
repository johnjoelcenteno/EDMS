using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Mappers;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application;

public record QueryRecordTypesRequest(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

public class QueryRecordTypes : IRequestHandler<QueryRecordTypesRequest, DataSourceResult>
{
    private readonly IReadRepository _readRepository;

    public QueryRecordTypes(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<DataSourceResult> Handle(QueryRecordTypesRequest request, CancellationToken cancellationToken)
    {
        var result = _readRepository
            .RecordTypesView
            .Select(x => RecordTypeMappers.Map(x))
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}
