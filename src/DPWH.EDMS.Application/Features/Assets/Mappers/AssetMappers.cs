using DPWH.EDMS.Domain.Entities;
using System.Linq.Expressions;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.Application.Features.Assets.Queries;

namespace DPWH.EDMS.Application.Features.Assets.Mappers;
public static class AssetMappers
{
    public static AssetModel MapToModel(Asset entity)
    {
        return new AssetModel
        {
            Id = entity.Id,
            PropertyId = entity.PropertyId,
            BuildingId = entity.BuildingId,
            Name = entity.Name,
            Status = EnumExtensions.GetDescriptionFromValue<AssetStatus>(entity.Status),
            PropertyStatus = entity.PropertyStatus,
            ImplementingOffice = entity.ImplementingOffice,
            RequestingOffice = entity.RequestingOffice,
            Agency = entity.Agency,
            AttachedAgency = entity.AttachedAgency,
            Group = entity.Group,
            Region = entity.Region,
            RegionId = entity.RegionId,
            Province = entity.Province,
            ProvinceId = entity.ProvinceId,
            CityOrMunicipality = entity.CityOrMunicipality,
            CityOrMunicipalityId = entity.CityOrMunicipalityId,
            Barangay = entity.Barangay,
            BarangayId = entity.BarangayId,
            ZipCode = entity.ZipCode,
            StreetAddress = entity.StreetAddress,
            Longitude = new LongLatFormat
            {
                Degrees = entity.LongDegrees,
                Minutes = entity.LongMinutes,
                Seconds = entity.LongSeconds,
                Direction = entity.LongDirection
            },
            Latitude = new LongLatFormat
            {
                Degrees = entity.LatDegrees,
                Minutes = entity.LatMinutes,
                Seconds = entity.LatSeconds,
                Direction = entity.LatDirection
            },
            LotArea = entity.LotArea,
            FloorArea = entity.FloorArea,
            Floors = entity.Floors,
            YearConstruction = entity.YearConstruction,
            MonthConstruction = entity.MonthConstruction,
            DayConstruction = entity.DayConstruction,
            ConstructionType = entity.ConstructionType,
            LotStatus = entity.LotStatus,
            BuildingStatus = entity.BuildingStatus,
            ZonalValue = entity.ZonalValue,
            BookValue = entity.BookValue,
            AppraisedValue = entity.AppraisedValue,
            OldId = entity.OldId,
            Remarks = entity.Remarks,
            Created = entity.Created,
            CreatedBy = entity.CreatedBy,
            LastModified = entity.LastModified,
            LastModifiedBy = entity.LastModifiedBy,
            Images = entity.Images?.Select(doc => new AssetImageModel
            {
                Id = doc.Id,
                AssetId = doc.AssetId,
                Filename = doc.Filename,
                Category = doc.Category,
                DocumentType = doc.DocumentType,
                Description = doc.Description,
                Uri = doc.Uri,
                Longitude = new LongLatFormat
                {
                    Degrees = entity.LongDegrees,
                    Minutes = entity.LongMinutes,
                    Seconds = entity.LongSeconds,
                    Direction = entity.LongDirection
                },
                Latitude = new LongLatFormat
                {
                    Degrees = entity.LatDegrees,
                    Minutes = entity.LatMinutes,
                    Seconds = entity.LatSeconds,
                    Direction = entity.LatDirection

                },
                FileSize = doc.FileSize,
                View = doc.View,
                Created = doc.Created,
                CreatedBy = doc.CreatedBy,
                LastModified = doc.LastModified,
                LastModifiedBy = doc.LastModifiedBy
            }).ToList() ?? new List<AssetImageModel>(),
            Files = entity.Files?.Select(doc => new AssetFileModel
            {
                Id = doc.Id,
                AssetId = doc.AssetId,
                Filename = doc.Filename,
                Category = doc.Category,
                DocumentType = doc.DocumentType,
                DocumentNo = doc.DocumentNo,
                DocumentTypeOthers = doc.DocumentTypeOthers,
                OtherRelatedDocuments = doc.OtherRelatedDocuments,
                Description = doc.Description,
                Uri = doc.Uri,
                FileSize = doc.FileSize,
                Created = doc.Created,
                CreatedBy = doc.CreatedBy,
                LastModified = doc.LastModified,
                LastModifiedBy = doc.LastModifiedBy
            }).ToList() ?? new List<AssetFileModel>(),
            FinancialDetails = entity.FinancialDetails != null ? new FinancialDetailsModel
            {
                Id = entity.FinancialDetails.Id,
                PaymentDetails = entity.FinancialDetails.PaymentDetails,
                ORNumber = entity.FinancialDetails.ORNumber,
                PaymentDate = entity.FinancialDetails.PaymentDate,
                AmountPaid = entity.FinancialDetails.AmountPaid,
                Policy = entity.FinancialDetails.Policy,
                PolicyNumber = entity.FinancialDetails.PolicyNumber,
                PolicyID = entity.FinancialDetails.PolicyID,
                EffectivityStart = entity.FinancialDetails.EffectivityStart,
                EffectivityEnd = entity.FinancialDetails.EffectivityEnd,
                Particular = entity.FinancialDetails.Particular,
                Building = entity.FinancialDetails.Building,
                Content = entity.FinancialDetails.Content,
                Premium = entity.FinancialDetails.Premium,
                TotalPremium = entity.FinancialDetails.TotalPremium,
                Remarks = entity.FinancialDetails.Remarks,
                FinancialDetailsDocuments = entity.FinancialDetailsDocuments?.Select(files => new FinancialDetailsDocumentsModel
                {
                    Id = files.Id,
                    AssetId = files.AssetId,
                    Allocation = files.Allocation,
                    YearFunded = files.YearFunded,
                    FileName = files.FileName,
                    FileSize = files.FileSize,
                    Uri = files.Uri,
                    Created = files.Created,
                    CreatedBy = files.CreatedBy,
                    LastModified = files.LastModified,
                    LastModifiedBy = files.LastModifiedBy
                }).ToList()
            } : new FinancialDetailsModel()
        };
    }

    public static Expression<Func<Asset, AssetModel>> MapToModelExpression()
    {
        return entity => new AssetModel
        {
            Id = entity.Id,
            PropertyId = entity.PropertyId,
            BuildingId = entity.BuildingId,
            Name = entity.Name,
            Status = EnumExtensions.GetDescriptionFromValue<AssetStatus>(entity.Status),
            PropertyStatus = entity.PropertyStatus,
            AttachedAgency = entity.AttachedAgency,
            Agency = entity.Agency,
            Group = entity.Group,
            ImplementingOffice = entity.ImplementingOffice,
            RequestingOffice = entity.RequestingOffice,
            Region = entity.Region,
            RegionId = entity.RegionId,
            Province = entity.Province,
            ProvinceId = entity.ProvinceId,
            CityOrMunicipality = entity.CityOrMunicipality,
            CityOrMunicipalityId = entity.CityOrMunicipalityId,
            Barangay = entity.Barangay,
            BarangayId = entity.BarangayId,
            ZipCode = entity.ZipCode,
            StreetAddress = entity.StreetAddress,
            Longitude = new LongLatFormat
            {
                Degrees = entity.LongDegrees,
                Minutes = entity.LongMinutes,
                Seconds = entity.LongSeconds,
                Direction = entity.LongDirection
            },
            Latitude = new LongLatFormat
            {
                Degrees = entity.LatDegrees,
                Minutes = entity.LatMinutes,
                Seconds = entity.LatSeconds,
                Direction = entity.LatDirection

            },
            LotArea = entity.LotArea,
            FloorArea = entity.FloorArea,
            Floors = entity.Floors,
            YearConstruction = entity.YearConstruction,
            MonthConstruction = entity.MonthConstruction,
            DayConstruction = entity.DayConstruction,
            ConstructionType = entity.ConstructionType,
            LotStatus = entity.LotStatus,
            BuildingStatus = entity.BuildingStatus,
            ZonalValue = entity.ZonalValue,
            AppraisedValue = entity.AppraisedValue,
            BookValue = entity.BookValue,
            Created = entity.Created,
            CreatedBy = entity.CreatedBy,
            LastModified = entity.LastModified,
            LastModifiedBy = entity.LastModifiedBy,
            Images = entity.Images.Select(doc => new AssetImageModel
            {
                Id = doc.Id,
                AssetId = doc.AssetId,
                Filename = doc.Filename,
                Category = doc.Category,
                DocumentType = doc.DocumentType,
                Description = doc.Description,
                Uri = doc.Uri,
                Longitude = new LongLatFormat
                {
                    Degrees = entity.LongDegrees,
                    Minutes = entity.LongMinutes,
                    Seconds = entity.LongSeconds,
                    Direction = entity.LongDirection
                },
                Latitude = new LongLatFormat
                {
                    Degrees = entity.LatDegrees,
                    Minutes = entity.LatMinutes,
                    Seconds = entity.LatSeconds,
                    Direction = entity.LatDirection

                },
                FileSize = doc.FileSize,
                View = doc.View,
                Created = doc.Created,
                CreatedBy = doc.CreatedBy,
                LastModified = doc.LastModified,
                LastModifiedBy = doc.LastModifiedBy
            }).ToList(),
            Files = entity.Files.Select(doc => new AssetFileModel
            {
                Id = doc.Id,
                AssetId = doc.AssetId,
                Filename = doc.Filename,
                Category = doc.Category,
                DocumentType = doc.DocumentType,
                DocumentNo = doc.DocumentNo,
                DocumentTypeOthers = doc.DocumentTypeOthers,
                OtherRelatedDocuments = doc.OtherRelatedDocuments,
                Description = doc.Description,
                Uri = doc.Uri,
                FileSize = doc.FileSize,
                Created = doc.Created,
                CreatedBy = doc.CreatedBy,
                LastModified = doc.LastModified,
                LastModifiedBy = doc.LastModifiedBy
            }).ToList(),
            FinancialDetails = entity.FinancialDetails != null ? new FinancialDetailsModel
            {
                Id = entity.FinancialDetails.Id,
                PaymentDetails = entity.FinancialDetails.PaymentDetails,
                ORNumber = entity.FinancialDetails.ORNumber,
                PaymentDate = entity.FinancialDetails.PaymentDate,
                AmountPaid = entity.FinancialDetails.AmountPaid,
                Policy = entity.FinancialDetails.Policy,
                PolicyNumber = entity.FinancialDetails.PolicyNumber,
                PolicyID = entity.FinancialDetails.PolicyID,
                EffectivityStart = entity.FinancialDetails.EffectivityStart,
                EffectivityEnd = entity.FinancialDetails.EffectivityEnd,
                Particular = entity.FinancialDetails.Particular,
                Building = entity.FinancialDetails.Building,
                Content = entity.FinancialDetails.Content,
                Premium = entity.FinancialDetails.Premium,
                TotalPremium = entity.FinancialDetails.TotalPremium,
                Remarks = entity.FinancialDetails.Remarks,
                FinancialDetailsDocuments = entity.FinancialDetailsDocuments.Select(files => new FinancialDetailsDocumentsModel
                {
                    Id = files.Id,
                    AssetId = files.AssetId,
                    Allocation = files.Allocation,
                    YearFunded = files.YearFunded,
                    FileName = files.FileName,
                    FileSize = files.FileSize,
                    Uri = files.Uri,
                    Created = files.Created,
                    CreatedBy = files.CreatedBy,
                    LastModified = files.LastModified,
                    LastModifiedBy = files.LastModifiedBy,
                }).ToList()
            } : new FinancialDetailsModel()
        };
    }
}
