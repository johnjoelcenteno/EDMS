using DPWH.EDMS.Shared.Enums;

namespace DPWH.EDMS.Application.Features.Maintenance.Queries;

public class MaintenanceRequestModel
{
    public Guid Id { get; set; }
    public string RequestNumber { get; set; }
    public string? BuildingId { get; set; }
    public string? BuildingName { get; set; }
    public string PropertyCondition { get; set; }
    public Guid AssetId { get; set; }
    public string? Purpose { get; set; }
    public string Status { get; set; }
    public int PhotosPerArea { get; set; }
    public bool? IsPhotosRequired { get; set; }
    public string? FurtherInstructions { get; set; }
    public decimal? RequestedAmount { get; set; }
    public string? PurposeProjectName { get; set; }
    public IEnumerable<MaintenanceRequestBuildingComponentsModel> BuildingComponents { get; set; }
    public IEnumerable<MaintenanceRequestDocumentModel>? Documents { get; set; }

}

public class MaintenanceRequestBuildingComponentsModel
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public List<MaintenanceSubComponents> SubCategories { get; set; }
}

public class MaintenanceSubComponents
{
    public string SubCategory { get; set; }
    public bool ForRepair { get; set; }
    public int? Rating { get; set; }
    public string? Particular { get; set; }
}

public class MaintenanceRequestDocumentModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
    public long? FileSize { get; set; }
    public string Uri { get; set; }
}