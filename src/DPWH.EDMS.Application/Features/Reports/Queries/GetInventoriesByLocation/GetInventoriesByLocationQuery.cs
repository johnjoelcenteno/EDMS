using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetInventoriesByLocation;

public record GetInventoriesByLocationQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetInventoriesByLocationHandler : IRequestHandler<GetInventoriesByLocationQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetInventoriesByLocationHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetInventoriesByLocationQuery request, CancellationToken cancellationToken)
    {
        var result = _repository
            .AssetsView
            .Select(property => new InventoryReportByLocation
            {
                BuildingId = property.BuildingId,
                BuildingName = property.Name,
                PropertyId = property.PropertyId,
                Region = property.Region,
                RegionId = property.RegionId,
                Province = property.Province,
                ProvinceId = property.ProvinceId,
                CityOrMunicipality = property.CityOrMunicipality,
                CityOrMunicipalityId = property.CityOrMunicipalityId,
                Barangay = property.Barangay,
                BarangayId = property.BarangayId,
                ZipCode = property.ZipCode,
                CreatedBy = property.CreatedBy,
                Created = property.Created,
                LastModifiedBy = property.LastModifiedBy,
                LastModified = property.LastModified
            })
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}