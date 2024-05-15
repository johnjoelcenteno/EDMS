using DPWH.EDMS.Application.Models;

namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.RentalRates;

public class GetRentalRatesResult : AuditableModel
{
    public string? Region { get; set; }
    public string? ImplementingOffice { get; set; }
    public string? BuildingName { get; set; }
    public string? Location { get; set; }
    public LocationAndSiteConditions? LocationAndSiteConditions { get; set; }
    public NeighborhoodData? NeighborhoodData { get; set; }
    public Building? Building { get; set; }
    public FreeServicesAndFacilities? FreeServicesAndFacilities { get; set; }
    public decimal LocationAndSiteConditionsFactorValue { get; set; }
    public decimal NeighborhoodDataFactorValue { get; set; }
    public decimal BuildingFactorValue { get; set; }
    public decimal FreeServicesAndFacilitiesFactorValue { get; set; }
    public decimal TotalFactorValue { get; set; }
}

public class LocationAndSiteConditions
{
    public LocationAndSiteConditions(decimal accessibility, decimal topographyAndDrainage, decimal sideWalkAndShed, decimal parkingSpace, decimal economicPotentiality, decimal landClassification, decimal otherAmenities)
    {
        Accessibility = accessibility;
        TopographyAndDrainage = topographyAndDrainage;
        SideWalkAndShed = sideWalkAndShed;
        ParkingSpace = parkingSpace;
        EconomicPotentiality = economicPotentiality;
        LandClassification = landClassification;
        OtherAmenities = otherAmenities;
        Total = CalculateTotal(accessibility + topographyAndDrainage + sideWalkAndShed + parkingSpace + economicPotentiality + landClassification + otherAmenities);
    }

    public decimal Accessibility { get; set; }
    public decimal TopographyAndDrainage { get; set; }
    public decimal SideWalkAndShed { get; set; }
    public decimal ParkingSpace { get; set; }
    public decimal EconomicPotentiality { get; set; }
    public decimal LandClassification { get; set; }
    public decimal OtherAmenities { get; set; }
    public decimal Total { get; set; }

    private static decimal CalculateTotal(params decimal[] values)
    {
        return values.Sum();
    }
}

public class NeighborhoodData
{
    public NeighborhoodData(decimal prevailingRentalRates, decimal sanitation, decimal adverseInfluence, decimal propertyUtilization, decimal policeAndFireStation, decimal cafeteria, decimal bankingPostalTelecom)
    {
        PrevailingRentalRates = prevailingRentalRates;
        Sanitation = sanitation;
        AdverseInfluence = adverseInfluence;
        PropertyUtilization = propertyUtilization;
        PoliceAndFireStation = policeAndFireStation;
        Cafeteria = cafeteria;
        BankingPostalTelecom = bankingPostalTelecom;
        Total = CalculateTotal(prevailingRentalRates + sanitation + adverseInfluence + propertyUtilization + policeAndFireStation + cafeteria + bankingPostalTelecom);
    }

    public decimal PrevailingRentalRates { get; set; }
    public decimal Sanitation { get; set; }
    public decimal AdverseInfluence { get; set; }
    public decimal PropertyUtilization { get; set; }
    public decimal PoliceAndFireStation { get; set; }
    public decimal Cafeteria { get; set; }
    public decimal BankingPostalTelecom { get; set; }
    public decimal Total { get; set; }

    private static decimal CalculateTotal(params decimal[] values)
    {
        return values.Sum();
    }
}

public class Building
{
    public Building(decimal structuralCondition, decimal module, decimal roomArrangement, decimal circulation, decimal lightAndVentilation, decimal spaceRequirements, decimal waterSupply, decimal lightingSystem, decimal elevators, decimal fireEscapes, decimal fireFightingEquipment, decimal maintenance, decimal alternatives)
    {
        StructuralCondition = structuralCondition;
        Module = module;
        RoomArrangement = roomArrangement;
        Circulation = circulation;
        LightAndVentilation = lightAndVentilation;
        SpaceRequirements = spaceRequirements;
        WaterSupply = waterSupply;
        LightingSystem = lightingSystem;
        Elevators = elevators;
        FireEscapes = fireEscapes;
        FireFightingEquipment = fireFightingEquipment;
        Maintenance = maintenance;
        Alternatives = alternatives;
        Total = CalculateTotal(structuralCondition + module + roomArrangement + circulation + lightAndVentilation + spaceRequirements + waterSupply + lightingSystem + elevators + fireEscapes + fireFightingEquipment + maintenance + alternatives);
    }

    public decimal StructuralCondition { get; set; }
    public decimal Module { get; set; }
    public decimal RoomArrangement { get; set; }
    public decimal Circulation { get; set; }
    public decimal LightAndVentilation { get; set; }
    public decimal SpaceRequirements { get; set; }
    public decimal WaterSupply { get; set; }
    public decimal LightingSystem { get; set; }
    public decimal Elevators { get; set; }
    public decimal FireEscapes { get; set; }
    public decimal FireFightingEquipment { get; set; }
    public decimal Maintenance { get; set; }
    public decimal Alternatives { get; set; }
    public decimal Total { get; set; }

    private static decimal CalculateTotal(params decimal[] values)
    {
        return values.Sum();
    }
}

public class FreeServicesAndFacilities
{
    public FreeServicesAndFacilities(decimal janitorial, decimal airconditioning, decimal repairAndMaintenance, decimal waterLightConsumption, decimal securedParkingSpace)
    {
        Janitorial = janitorial;
        Airconditioning = airconditioning;
        RepairAndMaintenance = repairAndMaintenance;
        WaterLightConsumption = waterLightConsumption;
        SecuredParkingSpace = securedParkingSpace;
        Total = CalculateTotal(janitorial + airconditioning + repairAndMaintenance + waterLightConsumption + securedParkingSpace);
    }

    public decimal Janitorial { get; set; }
    public decimal Airconditioning { get; set; }
    public decimal RepairAndMaintenance { get; set; }
    public decimal WaterLightConsumption { get; set; }
    public decimal SecuredParkingSpace { get; set; }
    public decimal Total { get; set; }

    private static decimal CalculateTotal(params decimal[] values)
    {
        return values.Sum();
    }
}