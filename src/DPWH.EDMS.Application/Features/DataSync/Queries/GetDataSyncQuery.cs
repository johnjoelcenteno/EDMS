using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.DataSync.Queries;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.DataSync.Queries;

public record GetDataSyncQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetDataSyncHandler : IRequestHandler<GetDataSyncQuery, DataSourceResult>
{
    private readonly IReadRepository _readRepository;

    public GetDataSyncHandler(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<DataSourceResult> Handle(GetDataSyncQuery request, CancellationToken cancellationToken)
    {
        var result = _readRepository.DataSyncLogsView
            .OrderByDescending(x => x.Created)
            .Select(e => new GetDataSyncQueryResult
            {
                Id = e.Id,
                Type = e.Type,
                Result = e.Result,
                Description = e.Description,
                Created = e.Created,
                CreatedBy = e.CreatedBy
            })
            .ToDataSourceResult(request.DataSourceRequest);

        return result;
    }
}
