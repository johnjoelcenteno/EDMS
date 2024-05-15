using DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionRequests;
using DPWH.EDMS.Domain.Entities;
using System.Linq.Expressions;

namespace DPWH.EDMS.Application.Features.Inspections.Mappers;

public static class InspectionRequestMappers
{
    public static Expression<Func<InspectionRequest, InspectionRequestModel>> MapToModelExpression()
    {
        return entity => new InspectionRequestModel
        {
            Id = entity.Id,
            AssetId = entity.AssetId,
            BuildingId = entity.Asset.BuildingId ?? entity.RentalRateProperty.RentalRateNumber,
            BuildingName = entity.Asset.Name ?? entity.RentalRateProperty.PropertyName,
            PropertyCondition = entity.Asset.PropertyStatus,
            Status = entity.Status,
            RentalRateId = entity.RentalRateProperty.Id,
            Purpose = entity.Purpose,
            Schedule = entity.Schedule,
            Deadline = entity.Deadline,
            EmployeeId = entity.EmployeeId,
            EmployeeName = entity.EmployeeName,
            PhotosPerArea = entity.PhotosPerArea,
            IsPhotosRequired = entity.IsPhotosRequired,
            FurtherInstructions = entity.Instructions,
            BuildingComponents = entity.InspectionRequestBuildingComponents
                                    .GroupBy(c => c.Category)
                                    .Select(group => new InspectionRequestBuildingComponentsModel
                                    {
                                        Id = group.First().Id,
                                        Category = group.Key,
                                        Status = group.First().IsUpdated,
                                        SubCategories = group.Select(c => new SubComponents
                                        {
                                            Id = c.Id,
                                            SubCategory = c.SubCategory,
                                            ForRepair = c.ForRepair,
                                            Rating = c.Rating,
                                            Particular = c.Particular,
                                            Images = group.Where(image => image.Images.InspectionRequestBuildingComponentId == c.Id)
                                                .Select(image => new SubComponentsImage
                                                {
                                                    Id = image.Images.Id,
                                                    Filename = image.Images.Filename,
                                                    Uri = image.Images.Uri,
                                                    FileSize = image.Images.FileSize
                                                }).ToList(),
                                        }).ToList(),
                                    }).ToList(),
            Documents = new InspectionRequestDocumentModel
            {
                FileName = entity.Documents.Filename,
                Name = entity.Documents.Name,
                Uri = entity.Documents.Uri
            },
            ProjectMonitorings = new InspectionProjectMonitoringModel
            {
                Id = entity.InspectionRequestProjectMonitoring.Id,
                ContractId = entity.ProjectMonitoring.ContractId,
                ProjectName = entity.ProjectMonitoring.ProjectName,
                ContractCost = entity.ProjectMonitoring.ContractCost,
                Month = entity.InspectionRequestProjectMonitoring.Month,
                Year = entity.InspectionRequestProjectMonitoring.Year,
                RevisedContractCost = entity.InspectionRequestProjectMonitoring.RevisedContractCost,
                RevisedExpiryDate = entity.InspectionRequestProjectMonitoring.RevisedExpiryDate,
                TotalProjectDuration = entity.InspectionRequestProjectMonitoring.TotalProjectDuration,
                Disbursement = entity.InspectionRequestProjectMonitoring.Disbursement,
                Balance = entity.InspectionRequestProjectMonitoring.Balance,
                FinancialPlanned = entity.InspectionRequestProjectMonitoring.FinancialPlanned,
                FinancialActual = entity.InspectionRequestProjectMonitoring.FinancialActual,
                FinancialRevised = entity.InspectionRequestProjectMonitoring.FinancialRevised,
                PhysicalPlanned = entity.InspectionRequestProjectMonitoring.PhysicalPlanned,
                PhysicalActual = entity.InspectionRequestProjectMonitoring.PhysicalActual,
                PhysicalRevised = entity.InspectionRequestProjectMonitoring.PhysicalRevised,
                PhysicalSlippage = entity.InspectionRequestProjectMonitoring.PhysicalSlippage,
                ProjectMonitoringBuildingComponents = entity.InspectionRequestProjectMonitoring.InspectionRequestProjectMonitoringScopes
                                                    .GroupBy(c => new { c.Category }) // Group by both Category and Subcategory
                                                    .Select(group => new ProjectMonitoringBuildingComponentsModel
                                                    {
                                                        Category = group.Key.Category,
                                                        Subcategories = group.GroupBy(sub => sub.Subcategory)
                                                            .Select(subGroup => new ProjectMonitoringSubcategoryModel
                                                            {
                                                                Subcategory = subGroup.Key,
                                                                Items = subGroup.Select(item => new ProjectMonitoringItemsModel
                                                                {
                                                                    Id = item.Id,
                                                                    ItemNo = item.ItemNo,
                                                                    Description = item.Description,
                                                                    Total = item.Total,
                                                                    FinancialPlanned = item.FinancialPlanned,
                                                                    FinancialActual = item.FinancialActual,
                                                                    FinancialRevised = item.FinancialRevised,
                                                                    PhysicalPlanned = item.PhysicalPlanned,
                                                                    PhysicalRelativePlanned = item.PhysicalRelativePlanned,
                                                                    PhysicalRelativeActual = item.PhysicalRelativeActual,
                                                                    PhysicalActual = item.PhysicalActual,
                                                                    PhysicalRelativeRevised = item.PhysicalRelativeRevised,
                                                                    PhysicalRevised = item.PhysicalRevised,
                                                                    Remarks = item.Remarks,
                                                                    Images = item.InspectionRequestProjectMonitoringScopesImage.Select(i => new ProjectMonitoringItemsImageModel
                                                                    {
                                                                        Id = i.Id,
                                                                        Filename = i.Filename,
                                                                        Uri = i.Uri,
                                                                        FileSize = i.FileSize
                                                                    }).ToList()
                                                                })
                                                            })
                                                    })
            }
        };
    }
}
