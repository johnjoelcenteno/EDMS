using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;

public class MaintenanceRequest : EntityBase
{
    public static MaintenanceRequest Create(
        Guid assetId,
        MaintenanceRequestStatus status,
        string? purpose,
        int photosPerArea,
        bool? isPhotosRequired,
        string? instructions,
        decimal requestedAmount,
        string purposeProjectName,
        string? requestNumber,
        string createdBy)
    {
        var inspection = new MaintenanceRequest
        {
            Id = Guid.NewGuid(),
            AssetId = assetId,
            Purpose = purpose,
            Status = status.ToString(),
            PhotosPerArea = photosPerArea,
            IsPhotosRequired = isPhotosRequired,
            Instructions = instructions,
            RequestedAmount = requestedAmount,
            PurposeProjectName = purposeProjectName,
            RequestNumber = requestNumber
        };
        inspection.SetCreated(createdBy);
        return inspection;
    }
    public void UpdateDetails(
        MaintenanceRequestStatus status,
        string? purpose,
        int photosPerArea,
        bool? isPhotosRequired,
        string? instructions,
        decimal requestedAmount,
        string purposeProjectName,
        string modifiedBy)
    {
        Status = status.ToString();
        Purpose = purpose;
        PhotosPerArea = photosPerArea;
        IsPhotosRequired = isPhotosRequired;
        Instructions = instructions;
        RequestedAmount = requestedAmount;
        PurposeProjectName = purposeProjectName;

        SetModified(modifiedBy);
    }

    [ForeignKey("AssetId")]
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; }
    public string Status { get; set; }
    public string? Purpose { get; set; }
    public int PhotosPerArea { get; set; }
    public bool? IsPhotosRequired { get; set; }
    public string? Instructions { get; set; }
    public virtual IList<MaintenanceRequestBuildingComponent>? MaintenanceRequestBuildingComponents { get; set; }
    public virtual IList<MaintenanceRequestDocument>? Documents { get; set; }
    public decimal? RequestedAmount { get; set; }
    public string? PurposeProjectName { get; set; }
    public string? RequestNumber { get; set; }
}
