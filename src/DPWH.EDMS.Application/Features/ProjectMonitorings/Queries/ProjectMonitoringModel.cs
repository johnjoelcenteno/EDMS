using Microsoft.Identity.Client;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Queries;

public class ProjectMonitoringModel
{
    public Guid Id { get; set; }
    public string MaintenanceRequestNumber { get; set; }
    public string ContractId { get; set; }
    public string BuildingId { get; set; }
    public string ProjectName { get; set; }
    public string Status { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
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

public class ProjectMonitoringComponentsModel
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public string Subcategory { get; set; }
    public string? ItemNo { get; set; }
    public string? Description { get; set; }
    public decimal? Total { get; set; }
    public string? Quantity { get; set; }
    public string? Unit { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? TotalCost { get; set; }
    public decimal? FinancialPlanned { get; set; }
    public decimal? FinancialActual { get; set; }
    public decimal? FinancialRevised { get; set; }
    public decimal? PhysicalPlanned { get; set; }
    public decimal? PhysicalRelativePlanned { get; set; }
    public decimal? PhysicalRelativeActual { get; set; }
    public decimal? PhysicalActual { get; set; }
    public decimal? PhysicalRelativeRevised { get; set; }
    public decimal? PhysicalRevised { get; set; }
    public string? Remarks { get; set; }
    public List<ProjectMonitoringComponentsImageModel>? Images { get; set; }
}

public class ProjectMonitoringComponentsImageModel
{
    public Guid Id { get; set; }
    public string? Filename { get; set; }
    public string? Uri { get; set; }
    public long? FileSize { get; set; }
}
