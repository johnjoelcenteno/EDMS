using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Shared.Enums;

namespace DPWH.EDMS.Domain.Entities;

public class InspectionRequest : EntityBase
{
    public static InspectionRequest Create(
        Asset? asset,
        RentalRateProperty? rentalRateProperty,
        string purpose,
        InspectionRequestStatus status,
        DateTimeOffset? schedule,
        DateTimeOffset? deadline,
        string employeeId,
        string employeeName,
        int photosPerArea,
        bool? isPhotosRequired,
        string? instructions,
        ProjectMonitoring? projectMonitoring,
        MaintenanceRequest? maintenanceRequest,
        string createdBy)
    {
        var inspection = new InspectionRequest
        {
            Id = Guid.NewGuid(),
            RentalRatePropertyId = rentalRateProperty?.Id,
            AssetId = asset?.Id,
            Status = status.ToString(),
            Purpose = purpose,
            Schedule = schedule,
            Deadline = deadline,
            EmployeeId = employeeId,
            EmployeeName = employeeName,
            PhotosPerArea = photosPerArea,
            IsPhotosRequired = isPhotosRequired,
            Instructions = instructions,
            ProjectMonitoringId = projectMonitoring?.Id,
            MaintenanceRequestId = maintenanceRequest?.Id,
        };
        inspection.SetCreated(createdBy);
        return inspection;
    }

    public void UpdateDetails(
        InspectionRequestStatus status,
        string purpose,
        DateTimeOffset? schedule,
        DateTimeOffset? deadline,
        string employeeId,
        string employeeName,
        int photosPerArea,
        bool? isPhotosRequired,
        string? instructions,
        string modifiedBy)
    {
        Status = status.ToString();
        Purpose = purpose;
        Schedule = schedule;
        Deadline = deadline;
        EmployeeId = employeeId;
        EmployeeName = employeeName;
        PhotosPerArea = photosPerArea;
        IsPhotosRequired = isPhotosRequired;
        Instructions = instructions;

        SetModified(modifiedBy);
    }

    public void UpdateStatus(InspectionRequestStatus status, string modifiedBy)
    {
        Status = status.ToString();

        SetModified(modifiedBy);
    }

    //[ForeignKey("AssetId")]
    public Guid? AssetId { get; set; }
    public Asset? Asset { get; set; }
    public RentalRateProperty? RentalRateProperty { get; set; }
    public Guid? RentalRatePropertyId { get; set; }
    public MaintenanceRequest? MaintenanceRequest { get; set; }
    public Guid? MaintenanceRequestId { get; set; }
    public ProjectMonitoring? ProjectMonitoring { get; set; }
    public InspectionRequestProjectMonitoring? InspectionRequestProjectMonitoring { get; set; }
    public Guid? ProjectMonitoringId { get; set; }
    public string? Status { get; set; }
    public string Purpose { get; set; }
    public DateTimeOffset? Schedule { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public string? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public int PhotosPerArea { get; set; }
    public bool? IsPhotosRequired { get; set; }
    public string? Instructions { get; set; }
    public virtual IList<InspectionRequestBuildingComponent> InspectionRequestBuildingComponents { get; set; }
    public InspectionRequestDocument Documents { get; set; }
}
