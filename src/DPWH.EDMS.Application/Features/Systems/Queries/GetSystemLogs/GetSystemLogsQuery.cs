using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Systems.Mappers;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Systems.Queries.GetSystemLogs;

public record GetSystemLogsQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

public class GetSystemLogsHandler : IRequestHandler<GetSystemLogsQuery, DataSourceResult>
{
    private readonly IReadRepository _readRepository;

    public GetSystemLogsHandler(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<DataSourceResult> Handle(GetSystemLogsQuery request, CancellationToken cancellationToken)
    {
        var result = _readRepository
            .SystemLogsView
            .Select(SystemLogMappers.MapToModelExpression())
            .OrderByDescending(log => log.Created)
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}