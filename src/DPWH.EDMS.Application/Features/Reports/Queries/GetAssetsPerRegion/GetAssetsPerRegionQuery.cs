using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Reports.Queries;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsPerRegion;

public record GetAssetsPerRegionQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetAssetsPerRegionHandler : IRequestHandler<GetAssetsPerRegionQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetAssetsPerRegionHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetAssetsPerRegionQuery request, CancellationToken cancellationToken)
    {
        var result = _repository.GeolocationsView
            .Where(address => address.Type == "Region")
            .GroupJoin(
                _repository.AssetsView, address => address.Name,
                asset => asset.Region,
                (address, assets) => new { Address = address, AssetCount = assets.Count() })
            .Select(grouped => new AssestPerConditionModel
            {
                NameOfItem = grouped.Address.Name,
                NumberOfItems = grouped.AssetCount
            })
            .OrderByDescending(report => report.NumberOfItems)
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}