namespace DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRates;
public class RentalRatesModel
{
    public Guid Id { get; set; }
    public Guid RentalRatesPropertyId { get; set; }
    public string Status { get; set; }
    public LocationAndSiteConditions LocationAndSiteConditions { get; set; }
    public NeighborhoodData NeighborhoodData { get; set; }
    public Building Building { get; set; }
    public FreeServicesAndFacilities FreeServicesAndFacilities { get; set; }
    public decimal? FactorValue { get; set; }
    public decimal? DepreciationValue { get; set; }
    public decimal? CapitalizationRate { get; set; }
    public decimal? UnitConstructionCost { get; set; }
    public decimal? ReproductionCost { get; set; }
    public decimal? FormulaRate { get; set; }
    public decimal? RentalRates { get; set; }
    public decimal? Area { get; set; }
    public decimal? MonthlyRental { get; set; }
    public decimal? DepreciationRate { get; set; }
    public decimal? CapitalizationRatePercentage { get; set; }
    public List<RentalRatesFile> Files { get; set; }
}
public class LocationAndSiteConditions
{
    public int? Accessibility { get; set; }
    public int? TopographyAndDrainage { get; set; }
    public int? SideWalkAndShed { get; set; }
    public int? ParkingSpace { get; set; }
    public int? EconomicPotentiality { get; set; }
    public int? LandClassification { get; set; }
    public int? OtherAmenities { get; set; }
    public int? Total => Calculate.Total(
                     Accessibility,
                     TopographyAndDrainage,
                     SideWalkAndShed,
                     ParkingSpace,
                     EconomicPotentiality,
                     LandClassification,
                     OtherAmenities);
    public List<RentalRatesImage> Images { get; set; }
}
public class NeighborhoodData
{
    public int? PrevailingRentalRates { get; set; }
    public int? Sanitation { get; set; }
    public int? AdverseInfluence { get; set; }
    public int? PropertyUtilization { get; set; }
    public int? PoliceAndFireStation { get; set; }
    public int? Cafeteria { get; set; }
    public int? BankingPostalTelecom { get; set; }
    public int? Total => Calculate.Total(
                    PrevailingRentalRates,
                    Sanitation,
                    AdverseInfluence,
                    PropertyUtilization,
                    PoliceAndFireStation,
                    Cafeteria,
                    BankingPostalTelecom);
    public List<RentalRatesImage> Images { get; set; }
}
public class Building
{
    public int? StructuralCondition { get; set; }
    public int? Module { get; set; }
    public int? RoomArrangement { get; set; }
    public int? Circulation { get; set; }
    public int? LightAndVentilation { get; set; }
    public int? SpaceRequirements { get; set; }
    public int? WaterSupply { get; set; }
    public int? LightingSystem { get; set; }
    public int? Elevators { get; set; }
    public int? FireEscapes { get; set; }
    public int? FireFightingEquipments { get; set; }
    public int? Maintenance { get; set; }
    public int? Alternatives { get; set; }
    public int? Total => Calculate.Total(
                        StructuralCondition,
                        Module,
                        RoomArrangement,
                        Circulation,
                        LightAndVentilation,
                        SpaceRequirements,
                        WaterSupply,
                        LightingSystem,
                        Elevators,
                        FireEscapes,
                        FireFightingEquipments,
                        Maintenance,
                        Alternatives);
    public List<RentalRatesImage> Images { get; set; }
}
public class FreeServicesAndFacilities
{
    public int? Janitorial { get; set; }
    public int? Airconditioning { get; set; }
    public int? RepairAndMaintenance { get; set; }
    public int? WaterLightConsumption { get; set; }
    public int? SecuredParkingSpace { get; set; }
    public int? Total => Calculate.Total(
                        Janitorial,
                        Airconditioning,
                        RepairAndMaintenance,
                        WaterLightConsumption,
                        SecuredParkingSpace);
    public List<RentalRatesImage> Images { get; set; }
}

public class RentalRatesDocument
{
    public Guid Id { get; set; }
    public string? FileName { get; set; }
    public string? Uri { get; set; }
    public long? FileSize { get; set; }
}
public class RentalRatesImage : RentalRatesDocument
{
    public string? Group { get; set; }
}

public class RentalRatesFile : RentalRatesDocument
{
    public string? Name { get; set; }
}

public static class Calculate
{
    public static int? Total(params int?[] values) => values.Sum();
}