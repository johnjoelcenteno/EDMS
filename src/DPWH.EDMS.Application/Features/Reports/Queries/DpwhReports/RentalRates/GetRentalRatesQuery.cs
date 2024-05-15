using DPWH.EDMS.Application.Contracts.Persistence;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.RentalRates;

public record GetRentalRatesQuery(DataSourceRequest DataSourceRequest) : IRequest<DataSourceResult>;

internal sealed class GetRentalRatesHandler : IRequestHandler<GetRentalRatesQuery, DataSourceResult>
{
    private readonly IReadRepository _repository;

    public GetRentalRatesHandler(IReadRepository repository)
    {
        _repository = repository;
    }
    public Task<DataSourceResult> Handle(GetRentalRatesQuery request, CancellationToken cancellationToken)
    {
        var dummyData = CreateDummyRentalRates();
        var result = new DataSourceResult
        {
            Data = dummyData,
            Total = dummyData.Count
        };

        return Task.FromResult(result);
    }

    // TODO: Implement
    private static List<GetRentalRatesResult> CreateDummyRentalRates()
    {
        var dummyRentalRates = new List<GetRentalRatesResult>
        {
            new()
            {
                Region = "Region I",
                ImplementingOffice = "National Capital Region",
                BuildingName = "Test Building",
                Location = "Metro Manila",
                LocationAndSiteConditions = new LocationAndSiteConditions(25, 20, 15, 15, 10, 10, 5),
                NeighborhoodData = new NeighborhoodData(20, 20, 15, 15, 15, 10, 5),
                Building = new Building(30, 6, 6, 6, 6, 6, 6, 6, 6, 6, 5, 5, 6),
                FreeServicesAndFacilities = new FreeServicesAndFacilities(20, 20, 20, 20, 20),
            }
        };

        foreach (var rentalRate in dummyRentalRates)
        {
            rentalRate.LocationAndSiteConditionsFactorValue = rentalRate.LocationAndSiteConditions.Total * 0.2M;
            rentalRate.NeighborhoodDataFactorValue = rentalRate.NeighborhoodData.Total * 0.2M;
            rentalRate.BuildingFactorValue = rentalRate.Building.Total * 0.5M;
            rentalRate.FreeServicesAndFacilitiesFactorValue = rentalRate.FreeServicesAndFacilities.Total * 0.1M;
            rentalRate.TotalFactorValue = rentalRate.LocationAndSiteConditions.Total * 0.2M + rentalRate.NeighborhoodData.Total * 0.2M + rentalRate.Building.Total * 0.5M + rentalRate.FreeServicesAndFacilities.Total * 0.1M;
        }
        return dummyRentalRates;
    }
}
