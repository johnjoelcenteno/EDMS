using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Reports.Queries;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsPerBuildingStatus;

public record GetAssetsPerBuildingStatusQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetAssetsPerBuildingStatusHandler : IRequestHandler<GetAssetsPerBuildingStatusQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetAssetsPerBuildingStatusHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetAssetsPerBuildingStatusQuery request, CancellationToken cancellationToken)
    {
        var result = _repository
            .AssetsView
            .GroupBy(asset => asset.BuildingStatus)
            .Select(g => new AssetsPerBuildingStatus
            {
                NameOfItem = g.Key,
                NumberOfItems = g.Count()
            })
            .OrderByDescending(report => report.NumberOfItems)
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}