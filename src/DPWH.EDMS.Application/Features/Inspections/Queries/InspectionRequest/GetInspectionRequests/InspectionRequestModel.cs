namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionRequests;

public class InspectionRequestModel
{
    public Guid Id { get; set; }
    public string? BuildingId { get; set; }
    public string? BuildingName { get; set; }
    public string PropertyCondition { get; set; }
    public Guid? AssetId { get; set; }
    public Guid? RentalRateId { get; set; }
    public string Status { get; set; }
    public string? Purpose { get; set; }
    public DateTimeOffset? Schedule { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public string? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public int PhotosPerArea { get; set; }
    public bool? IsPhotosRequired { get; set; }
    public string? FurtherInstructions { get; set; }
    public IEnumerable<InspectionRequestBuildingComponentsModel> BuildingComponents { get; set; }
    public InspectionRequestDocumentModel Documents { get; set; }
    public InspectionProjectMonitoringModel ProjectMonitorings { get; set; }
}

public class InspectionRequestBuildingComponentsModel
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public bool? Status { get; set; }
    public List<SubComponents> SubCategories { get; set; }
}

public class InspectionRequestDocumentModel
{
    public string? FileName { get; set; }
    public string? Name { get; set; }
    public string Uri { get; set; }
}

public class SubComponents
{
    public Guid Id { get; set; }
    public string SubCategory { get; set; }
    public bool ForRepair { get; set; }
    public int? Rating { get; set; }
    public string? Particular { get; set; }
    public List<SubComponentsImage> Images { get; set; }
}

public class SubComponentsImage
{
    public Guid Id { get; set; }
    public string? Filename { get; set; }
    public string? Uri { get; set; }
    public long? FileSize { get; set; }
}

public class InspectionProjectMonitoringModel
{
    public Guid? Id { get; set; }
    public string ContractId { get; set; }
    public string ProjectName { get; set; }
    public decimal? Allocation { get; set; }
    public int? Month { get; set; }
    public int? Year { get; set; }
    public decimal? ContractCost { get; set; }
    public decimal? RevisedContractCost { get; set; }
    public DateTimeOffset? RevisedExpiryDate { get; set; }
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
    public IEnumerable<ProjectMonitoringBuildingComponentsModel>? ProjectMonitoringBuildingComponents { get; set; }
}

public class ProjectMonitoringBuildingComponentsModel
{
    public string? Category { get; set; }
    public IEnumerable<ProjectMonitoringSubcategoryModel> Subcategories { get; set; }
}

public class ProjectMonitoringSubcategoryModel
{
    public string? Subcategory { get; set; }
    public IEnumerable<ProjectMonitoringItemsModel> Items { get; set; }
}

public class ProjectMonitoringItemsModel
{
    public Guid Id { get; set; }
    public string? ItemNo { get; set; }
    public string? Description { get; set; }
    public decimal? Total { get; set; }
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
    public List<ProjectMonitoringItemsImageModel> Images { get; set; }
}
public class ProjectMonitoringItemsImageModel
{
    public Guid Id { get; set; }
    public string? Filename { get; set; }
    public string? Uri { get; set; }
    public long? FileSize { get; set; }
}
