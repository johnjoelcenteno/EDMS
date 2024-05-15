using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Enums;
using System.Linq.Expressions;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRates;

namespace DPWH.EDMS.Application.Features.Inspections.Mappers;

public static class RentalRatesMappers
{
    public static Expression<Func<RentalRate, RentalRatesModel>> MapToModelExpression()
    {
        return entity => new RentalRatesModel
        {
            Id = entity.Id,
            RentalRatesPropertyId = entity.RentalRatePropertyId,
            Status = entity.Status,
            LocationAndSiteConditions = new LocationAndSiteConditions
            {
                Accessibility = entity.Accessibility,
                TopographyAndDrainage = entity.Topography,
                SideWalkAndShed = entity.Sidewalk,
                ParkingSpace = entity.Parking,
                EconomicPotentiality = entity.EconomicPotentiality,
                LandClassification = entity.LandClassification,
                OtherAmenities = entity.OtherAmenities,
                Images = entity.Images
                        .Where(x => x.Group == RentalRateType.LocationAndSiteConditions.ToString())
                        .Select(doc => new RentalRatesImage
                        {
                            Id = doc.Id,
                            FileName = doc.Filename,
                            Group = EnumExtensions.GetDescriptionFromValue<RentalRateType>(doc.Group),
                            Uri = doc.Uri,
                            FileSize = doc.FileSize
                        }).ToList()

            },
            NeighborhoodData = new NeighborhoodData
            {
                PrevailingRentalRates = entity.PrevailingRentalRates,
                Sanitation = entity.Sanitation,
                AdverseInfluence = entity.AdverseInfluence,
                PropertyUtilization = entity.PropertyUtilization,
                PoliceAndFireStation = entity.PoliceFireStation,
                Cafeteria = entity.Cafeteria,
                BankingPostalTelecom = entity.Banking,
                Images = entity.Images
                        .Where(x => x.Group == RentalRateType.NeighborhoodData.ToString())
                        .Select(doc => new RentalRatesImage
                        {
                            Id = doc.Id,
                            FileName = doc.Filename,
                            Group = EnumExtensions.GetDescriptionFromValue<RentalRateType>(doc.Group),
                            Uri = doc.Uri,
                            FileSize = doc.FileSize
                        }).ToList()

            },
            Building = new Building
            {
                StructuralCondition = entity.StructuralCondition,
                Module = entity.Module,
                RoomArrangement = entity.RoomArrangement,
                Circulation = entity.Circulation,
                LightAndVentilation = entity.LightAndVentilation,
                SpaceRequirements = entity.SpaceRequirements,
                WaterSupply = entity.WaterSupply,
                LightingSystem = entity.LightingSystem,
                Elevators = entity.Elevators,
                FireEscapes = entity.FireEscapes,
                FireFightingEquipments = entity.FireFightingEquipments,
                Maintenance = entity.Maintenance,
                Alternatives = entity.Alternatives,
                Images = entity.Images
                        .Where(x => x.Group == RentalRateType.Building.ToString())
                        .Select(doc => new RentalRatesImage
                        {
                            Id = doc.Id,
                            FileName = doc.Filename,
                            Group = EnumExtensions.GetDescriptionFromValue<RentalRateType>(doc.Group),
                            Uri = doc.Uri,
                            FileSize = doc.FileSize
                        }).ToList()

            },
            FreeServicesAndFacilities = new FreeServicesAndFacilities
            {
                Janitorial = entity.Janitorial,
                Airconditioning = entity.Airconditioning,
                RepairAndMaintenance = entity.RepairMaintenance,
                WaterLightConsumption = entity.WaterLightConsumption,
                SecuredParkingSpace = entity.SecuredParkingSpace,
                Images = entity.Images
                        .Where(x => x.Group == RentalRateType.FreeServicesAndFacilities.ToString())
                        .Select(doc => new RentalRatesImage
                        {
                            Id = doc.Id,
                            FileName = doc.Filename,
                            Group = EnumExtensions.GetDescriptionFromValue<RentalRateType>(doc.Group),
                            Uri = doc.Uri,
                            FileSize = doc.FileSize
                        }).ToList()

            },
            FactorValue = entity.FactorValue,
            DepreciationValue = entity.DepreciationValue,
            CapitalizationRate = entity.CapitalizationRate,
            UnitConstructionCost = entity.UnitConstructionCost,
            ReproductionCost = entity.ReproductionCost,
            FormulaRate = entity.FormulaRate,
            RentalRates = entity.RentalRates,
            Area = entity.Area,
            MonthlyRental = entity.MonthlyRental,
            DepreciationRate = entity.DepreciationRate,
            CapitalizationRatePercentage = entity.CapitalizationRatePercentage,
            Files = entity.Files
                        .Where(x => x.Category == AssetDocumentCategory.File.ToString())
                        .Select(doc => new RentalRatesFile
                        {
                            Id = doc.Id,
                            FileName = doc.Filename,
                            Name = doc.Name,
                            Uri = doc.Uri,
                            FileSize = doc.FileSize
                        }).ToList()
        };
    }
}
