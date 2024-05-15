using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetInventoriesByArea;

public record GetInventoriesByAreaQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetInventoriesByAreaHandler : IRequestHandler<GetInventoriesByAreaQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetInventoriesByAreaHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetInventoriesByAreaQuery request, CancellationToken cancellationToken)
    {
        var result = _repository
            .AssetsView
            .Select(property => new InventoryReportByArea
            {
                BuildingId = property.BuildingId,
                BuildingName = property.Name,
                PropertyId = property.PropertyId,
                LotArea = property.LotArea,
                FloorArea = property.FloorArea,
                CreatedBy = property.CreatedBy,
                Created = property.Created,
                LastModifiedBy = property.LastModifiedBy,
                LastModified = property.LastModified
            })
            .OrderByDescending(property => property.LotArea)
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}