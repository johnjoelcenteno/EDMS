using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Enums;
using System.Linq.Expressions;
using DPWH.EDMS.Domain.Extensions;
using Microsoft.IdentityModel.Tokens;
using DPWH.EDMS.Application.Features.Maintenance.Queries;

namespace DPWH.EDMS.Application.Features.Maintenance.Mappers;

public static class MaintenanceRequestMappers
{

    public static MaintenanceRequestModel MapToModel(MaintenanceRequest entity)
    {
        return new MaintenanceRequestModel
        {
            Id = entity.Id,
            AssetId = entity.AssetId,
            RequestNumber = entity.RequestNumber,
            BuildingId = entity.Asset.BuildingId,
            PropertyCondition = entity.Asset.PropertyStatus,
            Purpose = entity.Purpose,
            BuildingName = entity.Asset.Name,
            Status = entity.Status,
            PhotosPerArea = entity.PhotosPerArea,
            IsPhotosRequired = entity.IsPhotosRequired,
            FurtherInstructions = entity.Instructions,
            RequestedAmount = !entity.RequestedAmount.Equals(null) ? entity.RequestedAmount : null,
            PurposeProjectName = !string.IsNullOrEmpty(entity.PurposeProjectName) ? entity.PurposeProjectName : null,

            BuildingComponents = entity.MaintenanceRequestBuildingComponents
                .GroupBy(c => c.Category) // Group by category
                .Select(g => new MaintenanceRequestBuildingComponentsModel
                {
                    Id = g.First().Id, // Take the Id from the first item in the group
                    Category = g.Key, // Category name
                    SubCategories = g.Select(c => new MaintenanceSubComponents
                    {
                        SubCategory = c.SubCategory,
                        ForRepair = c.ForRepair,
                        Rating = c.Rating,
                        Particular = c.Particular
                    }).ToList()
                }).ToList(),
            Documents = entity.Documents.Select(doc => new MaintenanceRequestDocumentModel
            {
                Id = doc.Id,
                Name = doc.Name,
                Group = doc.Group,
                Uri = doc.Uri,
                FileSize = doc.FileSize
            }),

        };
    }



    public static Expression<Func<MaintenanceRequest, MaintenanceRequestModel>> MapToModelExpression()
    {
        return entity => new MaintenanceRequestModel
        {
            Id = entity.Id,
            RequestNumber = entity.RequestNumber,
            AssetId = entity.AssetId,
            BuildingId = entity.Asset.BuildingId,
            BuildingName = !string.IsNullOrEmpty(entity.Asset.Name) ? entity.Asset.Name : null,
            PropertyCondition = entity.Asset.PropertyStatus,
            Status = entity.Status,
            Purpose = entity.Purpose,
            PhotosPerArea = entity.PhotosPerArea,
            IsPhotosRequired = entity.IsPhotosRequired,
            FurtherInstructions = entity.Instructions,
            RequestedAmount = !entity.RequestedAmount.Equals(null) ? entity.RequestedAmount : null,
            PurposeProjectName = !string.IsNullOrEmpty(entity.PurposeProjectName) ? entity.PurposeProjectName : null,

            BuildingComponents = entity.MaintenanceRequestBuildingComponents
                                    .GroupBy(c => c.Category)  // Group by category
                                    .Select(group => new MaintenanceRequestBuildingComponentsModel
                                    {
                                        Id = group.First().Id,
                                        Category = group.Key,  // Category name
                                        SubCategories = group.Select(c => new MaintenanceSubComponents
                                        {
                                            SubCategory = c.SubCategory,
                                            ForRepair = c.ForRepair,
                                            Rating = c.Rating,
                                            Particular = c.Particular
                                        }).ToList()
                                    }).ToList(),
            Documents = entity.Documents.Select(doc => new MaintenanceRequestDocumentModel
            {
                Id = doc.Id,
                Name = doc.Name,
                Group = doc.Group,
                Uri = doc.Uri,
                FileSize = doc.FileSize
            }),
        };
    }
}
