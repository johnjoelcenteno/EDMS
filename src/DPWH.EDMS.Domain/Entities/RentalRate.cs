using DPWH.EDMS.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;

public class RentalRate : EntityBase
{
    private RentalRate()
    {
    }

    private RentalRate(RentalRateProperty rentalRateProperty, string status, int? accessibility, int? topography, int? sidewalk, int? parking, int? economic, int? land, int? amenities, int? prevailing,
        int? sanitation, int? influence, int? utilization, int? police, int? cafeteria, int? banking, int? structural, int? module, int? room, int? circulation, int? ventilation, int?
        space, int? water, int? lighting, int? elevators, int? fire, int? firefighting, int? maintenance, int? alternatives, int? janitorial, int? aircon, int? repair, int? consumption, int? secured, decimal? factor, decimal? depreciation, decimal? capitalization, decimal? construction, decimal? reproduction, decimal? formula, decimal? rental, decimal? area, decimal? monthly, decimal? depreciationRatePercentage, decimal? capitalizationRatePercentage)
    {
        RentalRatePropertyId = rentalRateProperty.Id;
        Status = status;
        Accessibility = accessibility;
        Topography = topography;
        Sidewalk = sidewalk;
        Parking = parking;
        EconomicPotentiality = economic;
        LandClassification = land;
        OtherAmenities = amenities;
        PrevailingRentalRates = prevailing;
        Sanitation = sanitation;
        AdverseInfluence = influence;
        PropertyUtilization = utilization;
        PoliceFireStation = police;
        Cafeteria = cafeteria;
        Banking = banking;
        StructuralCondition = structural;
        Module = module;
        RoomArrangement = room;
        Circulation = circulation;
        LightAndVentilation = ventilation;
        SpaceRequirements = space;
        WaterSupply = water;
        LightingSystem = lighting;
        Elevators = elevators;
        FireEscapes = fire;
        FireFightingEquipments = firefighting;
        Maintenance = maintenance;
        Alternatives = alternatives;
        Janitorial = janitorial;
        Airconditioning = aircon;
        RepairMaintenance = repair;
        WaterLightConsumption = consumption;
        SecuredParkingSpace = secured;
        FactorValue = factor;
        DepreciationValue = depreciation;
        CapitalizationRate = capitalization;
        UnitConstructionCost = construction;
        ReproductionCost = reproduction;
        FormulaRate = formula;
        RentalRates = rental;
        Area = area;
        MonthlyRental = monthly;
        DepreciationRate = depreciationRatePercentage;
        CapitalizationRatePercentage = capitalizationRatePercentage;

    }

    public static RentalRate Create(RentalRateProperty rentalRateProperty, string status, int? accessibility, int? topography, int? sidewalk, int? parking, int? economic, int? land, int? amenities,
        int? prevailing, int? sanitation, int? influence, int? utilization, int? police, int? cafeteria, int? banking, int? structural, int? module, int? room, int? circulation, int? ventilation,
        int? space, int? water, int? lighting, int? elevators, int? fire, int? fireFighting, int? maintenance, int? alternatives, int? janitorial, int? aircon, int? repair, int? consumption, int? secured, decimal? factor, decimal? depreciation, decimal? capitalization, decimal? construction, decimal? reproduction, decimal? formula, decimal? rental, decimal? area, decimal? monthly, decimal? depreciationRatePercentage, decimal? capitalizationRatePercentage, string createdBy)
    {
        var entity = new RentalRate(rentalRateProperty, status, accessibility, topography, sidewalk, parking, economic, land, amenities, prevailing, sanitation, influence, utilization, police, cafeteria, banking, structural, module, room, circulation, ventilation, space, water, lighting, elevators, fire, fireFighting, maintenance, alternatives, janitorial, aircon, repair, consumption, secured, factor, depreciation, capitalization, construction, reproduction, formula, rental, area, monthly, depreciationRatePercentage, capitalizationRatePercentage);

        entity.SetCreated(createdBy);

        return entity;
    }

    public void Update(RentalRateProperty rentalRateProperty, string status, int? accessibility, int? topography, int? sidewalk, int? parking, int? economic, int? land, int? amenities,
        int? prevailing, int? sanitation, int? influence, int? utilization, int? police, int? cafeteria, int? banking, int? structural, int? module, int? room, int? circulation, int? ventilation,
        int? space, int? water, int? lighting, int? elevators, int? fire, int? fireFighting, int? maintenance, int? alternatives, int? janitorial, int? aircon, int? repair, int? consumption, int? secured, decimal? factor, decimal? depreciation, decimal? capitalization, decimal? construction, decimal? reproduction, decimal? formula, decimal? rental, decimal? area, decimal? monthly, decimal? depreciationRatePercentage, decimal? capitalizationRatePercentage, string modifiedBy)
    {
        RentalRatePropertyId = rentalRateProperty.Id;
        Status = status;
        Accessibility = accessibility;
        Topography = topography;
        Sidewalk = sidewalk;
        Parking = parking;
        EconomicPotentiality = economic;
        LandClassification = land;
        OtherAmenities = amenities;
        PrevailingRentalRates = prevailing;
        Sanitation = sanitation;
        AdverseInfluence = influence;
        PropertyUtilization = utilization;
        PoliceFireStation = police;
        Cafeteria = cafeteria;
        Banking = banking;
        StructuralCondition = structural;
        Module = module;
        RoomArrangement = room;
        Circulation = circulation;
        LightAndVentilation = ventilation;
        SpaceRequirements = space;
        WaterSupply = water;
        LightingSystem = lighting;
        Elevators = elevators;
        FireEscapes = fire;
        FireFightingEquipments = fireFighting;
        Maintenance = maintenance;
        Alternatives = alternatives;
        Janitorial = janitorial;
        Airconditioning = aircon;
        RepairMaintenance = repair;
        WaterLightConsumption = consumption;
        SecuredParkingSpace = secured;
        FactorValue = factor;
        DepreciationValue = depreciation;
        CapitalizationRate = capitalization;
        UnitConstructionCost = construction;
        ReproductionCost = reproduction;
        FormulaRate = formula;
        RentalRates = rental;
        Area = area;
        MonthlyRental = monthly;
        DepreciationRate = depreciationRatePercentage;
        CapitalizationRatePercentage = capitalizationRatePercentage;
        SetModified(modifiedBy);
    }

    [ForeignKey("RentalRatePropertyId")]
    public Guid RentalRatePropertyId { get; set; }
    public virtual IList<RentalRateImageDocument> Images { get; set; }
    public virtual IList<RentalRateFileDocument> Files { get; set; }
    public string Status { get; set; }
    public int? Accessibility { get; set; }
    public int? Topography { get; set; }
    public int? Sidewalk { get; set; }
    public int? Parking { get; set; }
    public int? EconomicPotentiality { get; set; }
    public int? LandClassification { get; set; }
    public int? OtherAmenities { get; set; }
    public int? PrevailingRentalRates { get; set; }
    public int? Sanitation { get; set; }
    public int? AdverseInfluence { get; set; }
    public int? PropertyUtilization { get; set; }
    public int? PoliceFireStation { get; set; }
    public int? Cafeteria { get; set; }
    public int? Banking { get; set; }
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
    public int? Janitorial { get; set; }
    public int? Airconditioning { get; set; }
    public int? RepairMaintenance { get; set; }
    public int? WaterLightConsumption { get; set; }
    public int? SecuredParkingSpace { get; set; }
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
}
