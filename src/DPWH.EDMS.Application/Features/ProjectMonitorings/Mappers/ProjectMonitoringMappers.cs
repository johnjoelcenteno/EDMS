using DPWH.EDMS.Application.Features.ProjectMonitorings.Queries;
using DPWH.EDMS.Domain.Entities;
using System.Linq.Expressions;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Mappers;

public static class ProjectMonitoringMappers
{
    public static Expression<Func<ProjectMonitoring, ProjectMonitoringModel>> MapToModelExpression()
    {
        return entity => new ProjectMonitoringModel
        {
            Id = entity.Id,
            MaintenanceRequestNumber = entity.MaintenanceRequestNumber,
            ContractId = entity.ContractId,
            BuildingId = entity.Asset.BuildingId,
            ProjectName = entity.ProjectName,
            Status = entity.Status,
            Month = entity.Month,
            Year = entity.Year,
            SAADate = entity.SAADate,
            SAANumber = entity.SAANumber,
            Allocation = entity.Allocation,
            ContractCost = entity.ContractCost,
            RevisedContractCost = entity.RevisedContractCost,
            StartDate = entity.StartDate,
            ExpiryDate = entity.ExpiryDate,
            RevisedExpiryDate = entity.RevisedExpiryDate,
            ProjectDuration = entity.ProjectDuration,
            TotalProjectDuration = entity.TotalProjectDuration,
            Disbursement = entity.Disbursement,
            Balance = entity.Balance,
            FinancialPlanned = entity.FinancialPlanned,
            FinancialRevised = entity.FinancialRevised,
            FinancialActual = entity.FinancialActual,
            PhysicalPlanned = entity.PhysicalPlanned,
            PhysicalRevised = entity.PhysicalRevised,
            PhysicalActual = entity.PhysicalActual,
            PhysicalSlippage = entity.PhysicalSlippage,
            Remarks = entity.Remarks,
            BuildingComponents = entity.InspectionRequestProjectMonitoringScopes
            .Select(c => new ProjectMonitoringComponentsModel
            {
                Id = c.Id,
                Category = c.Category,
                Subcategory = c.Subcategory,
                ItemNo = c.ItemNo,
                Description = c.Description,
                Total = c.Total,
                Quantity = c.Quantity,
                Unit = c.Unit,
                UnitCost = c.UnitCost,
                TotalCost = c.TotalCost,
                FinancialPlanned = c.FinancialPlanned,
                FinancialActual = c.FinancialActual,
                FinancialRevised = c.FinancialRevised,
                PhysicalPlanned = c.PhysicalPlanned,
                PhysicalRelativePlanned = c.PhysicalRelativePlanned,
                PhysicalRelativeActual = c.PhysicalRelativeActual,
                PhysicalActual = c.PhysicalActual,
                PhysicalRelativeRevised = c.PhysicalRelativeRevised,
                PhysicalRevised = c.PhysicalRevised,
                Remarks = c.Remarks,
                Images = c.InspectionRequestProjectMonitoringScopesImage.Select(i => new ProjectMonitoringComponentsImageModel
                {
                    Id = i.Id,
                    Filename = i.Filename,
                    Uri = i.Uri,
                    FileSize = i.FileSize
                }).ToList()
            }).ToList()
        };
    }

    public static ProjectMonitoringModel MapToModel(ProjectMonitoring entity)
    {
        return new ProjectMonitoringModel
        {
            Id = entity.Id,
            ContractId = entity.ContractId,
            ProjectName = entity.ProjectName,
            Status = entity.Status,
            Month = entity.Month,
            Year = entity.Year,
            SAADate = entity.SAADate,
            SAANumber = entity.SAANumber,
            Allocation = entity.Allocation,
            ContractCost = entity.ContractCost,
            RevisedContractCost = entity.RevisedContractCost,
            StartDate = entity.StartDate,
            ExpiryDate = entity.ExpiryDate,
            RevisedExpiryDate = entity.RevisedExpiryDate,
            ProjectDuration = entity.ProjectDuration,
            Disbursement = entity.Disbursement,
            Balance = entity.Balance,
            FinancialPlanned = entity.FinancialPlanned,
            FinancialRevised = entity.FinancialRevised,
            FinancialActual = entity.FinancialActual,
            PhysicalPlanned = entity.PhysicalPlanned,
            PhysicalRevised = entity.PhysicalRevised,
            PhysicalActual = entity.PhysicalActual,
            PhysicalSlippage = entity.PhysicalSlippage,
            Remarks = entity.Remarks,
            BuildingComponents = entity.InspectionRequestProjectMonitoringScopes
            .Select(c => new ProjectMonitoringComponentsModel
            {
                Id = c.Id,
                Category = c.Category,
                Subcategory = c.Subcategory,
                ItemNo = c.ItemNo,
                Description = c.Description,
                Total = c.Total,
                Quantity = c.Quantity,
                Unit = c.Unit,
                UnitCost = c.UnitCost,
                TotalCost = c.TotalCost,
                FinancialPlanned = c.FinancialPlanned,
                FinancialActual = c.FinancialActual,
                FinancialRevised = c.FinancialRevised,
                PhysicalPlanned = c.PhysicalPlanned,
                PhysicalRelativePlanned = c.PhysicalRelativePlanned,
                PhysicalRelativeActual = c.PhysicalRelativeActual,
                PhysicalActual = c.PhysicalActual,
                PhysicalRelativeRevised = c.PhysicalRelativeRevised,
                PhysicalRevised = c.PhysicalRevised,
                Remarks = c.Remarks,
                Images = c.InspectionRequestProjectMonitoringScopesImage.Select(i => new ProjectMonitoringComponentsImageModel
                {
                    Id = i.Id,
                    Filename = i.Filename,
                    Uri = i.Uri,
                    FileSize = i.FileSize
                }).ToList()
            }).ToList()
        };
    }
}
