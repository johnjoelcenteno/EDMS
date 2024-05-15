namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequest;

public class CreateInspectionRequestRequest
{
    public Guid AssetId { get; set; }
    public string Status { get; set; }
    public string Purpose { get; set; }
    public DateTimeOffset? Schedule { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public string? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public IEnumerable<string>? BuildingComponentIds { get; set; }
    public int MinNumPhotosPerArea { get; set; }
    public int MaxNumPhotosPerArea { get; set; }
    public string FurtherInstructions { get; set; }
}
