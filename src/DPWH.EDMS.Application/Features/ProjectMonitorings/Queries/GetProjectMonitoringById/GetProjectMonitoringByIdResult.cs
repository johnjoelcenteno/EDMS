using DPWH.EDMS.Application.Features.ProjectMonitorings.Queries;
using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetProjectMonitoringById;

public record GetProjectMonitoringByIdResult
{
    public GetProjectMonitoringByIdResult(ProjectMonitoringModel model)
    {
        Id = model.Id;
        MaintenanceRequestNumber = model.MaintenanceRequestNumber;
        ContractId = model.ContractId;
        BuildingId = model.BuildingId;
        ProjectName = model.ProjectName;
        Status = model.Status;
        SAADate = model.SAADate;
        SAANumber = model.SAANumber;
        Allocation = model.Allocation;
        ContractCost = model.ContractCost;
        RevisedContractCost = model.RevisedContractCost;
        StartDate = model.StartDate;
        ExpiryDate = model.ExpiryDate;
        RevisedExpiryDate = model.RevisedExpiryDate;
        ProjectDuration = model.ProjectDuration;
        TotalProjectDuration = model.TotalProjectDuration;
        Disbursement = model.Disbursement;
        Balance = model.Balance;
        FinancialPlanned = model.FinancialPlanned;
        FinancialRevised = model.FinancialRevised;
        FinancialActual = model.FinancialActual;
        PhysicalPlanned = model.PhysicalPlanned;
        PhysicalRevised = model.PhysicalRevised;
        PhysicalActual = model.PhysicalActual;
        PhysicalSlippage = model.PhysicalSlippage;
        Remarks = model.Remarks;
        BuildingComponents = model.BuildingComponents?
            .GroupBy(c => c.Category)
            .SelectMany(g => g.Select((c, index) => new ProjectMonitoringComponentsModel
            {
                Id = c.Id,
                Category = g.Key,
                Subcategory = c.Subcategory,
                Description = c.Description,
                ItemNo = c.ItemNo,
                Total = c.Total,
                Quantity = c.Quantity,
                Unit = c.Unit,
                UnitCost = c.UnitCost,
                TotalCost = c.TotalCost,
                PhysicalPlanned = c.PhysicalPlanned,
                FinancialPlanned = c.FinancialPlanned,
                FinancialActual = c.FinancialActual,
                FinancialRevised = c.FinancialRevised,
                PhysicalRelativePlanned = c.PhysicalRelativePlanned,
                PhysicalRelativeActual = c.PhysicalRelativeActual,
                PhysicalActual = c.PhysicalActual,
                PhysicalRelativeRevised = c.PhysicalRelativeRevised,
                PhysicalRevised = c.PhysicalRevised,
                Remarks = c.Remarks,
                Images = c.Images?.Select(i => new ProjectMonitoringComponentsImageModel
                {
                    Id = i.Id,
                    Filename = i.Filename,
                    Uri = i.Uri,
                    FileSize = i.FileSize
                }).ToList()
            }))
            .ToList();
    }
    public Guid Id { get; set; }
    public string MaintenanceRequestNumber { get; set; }
    public string ContractId { get; set; }
    public string BuildingId { get; set; }
    public string ProjectName { get; set; }
    public string Status { get; set; }
    public DateTimeOffset SAADate { get; set; }
    public string SAANumber { get; set; }
    public decimal? Allocation { get; set; }
    public decimal? ContractCost { get; set; }
    public decimal? RevisedContractCost { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
    public DateTimeOffset? RevisedExpiryDate { get; set; }
    public string ProjectDuration { get; set; }
    public int? TotalProjectDuration { get; set; }
    public decimal? Disbursement { get; set; }
    public decimal? Balance { get; set; }
    public decimal? FinancialPlanned { get; set; }
    public decimal? FinancialRevised { get; set; }
    public decimal? FinancialActual { get; set; }
    public decimal? PhysicalPlanned { get; set; }
    public decimal? PhysicalRevised { get; set; }
    public decimal? PhysicalActual { get; set; }
    public decimal? PhysicalSlippage { get; set; }
    public string? Remarks { get; set; }
    public IEnumerable<ProjectMonitoringComponentsModel>? BuildingComponents { get; set; }
}
