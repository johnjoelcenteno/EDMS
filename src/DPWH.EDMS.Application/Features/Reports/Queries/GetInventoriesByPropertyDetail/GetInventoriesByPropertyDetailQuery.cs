using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Reports.Queries;
using DPWH.EDMS.Domain.Extensions;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetInventoriesByPropertyDetail;

public record GetInventoriesByPropertyDetailQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetInventoriesByPropertyDetailHandler : IRequestHandler<GetInventoriesByPropertyDetailQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetInventoriesByPropertyDetailHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public Task<DataSourceResult> Handle(GetInventoriesByPropertyDetailQuery request, CancellationToken cancellationToken)
    {
        var result = _repository
            .AssetsView
            .Select(property => new InventoryReportByPropertyDetails
            {
                BuildingId = property.BuildingId,
                BuildingName = property.Name,
                PropertyId = property.PropertyId,
                Region = property.Region,
                ImplementingOffice = property.ImplementingOffice,
                RequestingOffice = property.RequestingOffice,
                Agency = property.Agency,
                Group = property.Group,
                Province = property.Province,
                CityOrMunicipality = property.CityOrMunicipality,
                ZipCode = property.ZipCode,
                StreetAddress = property.StreetAddress,
                PropertyCondition = property.PropertyStatus,
                BuildingStatus = property.BuildingStatus,
                LotArea = property.LotArea,
                FloorArea = property.FloorArea,
                Floors = property.Floors,
                ConstructionType = property.ConstructionType,
                YearConstruction = property.YearConstruction,
                BookValue = property.BookValue,
                AppraisedValue = property.AppraisedValue,
                LotStatus = property.LotStatus,
                ZonalValue = property.ZonalValue,
                Created = property.Created,
                CreatedBy = property.CreatedBy,
                LastModified = property.LastModified,
                LastModifiedBy = property.LastModifiedBy
            })
            .ToDataSourceResult(request.DataSourceRequest.FixSerialization());

        return Task.FromResult(result);
    }
}