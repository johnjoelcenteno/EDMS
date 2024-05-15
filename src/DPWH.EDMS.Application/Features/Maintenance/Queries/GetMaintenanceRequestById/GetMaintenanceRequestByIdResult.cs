using DPWH.EDMS.Application.Features.Maintenance.Queries;
using DPWH.EDMS.Domain.Enums;
using DPWH.EDMS.Domain.Extensions;
using DPWH.EDMS.Shared.Enums;

namespace DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequestById;

public record GetMaintenanceRequestByIdResult
{
    public GetMaintenanceRequestByIdResult(MaintenanceRequestModel model)
    {
        Id = model.Id;
        AssetId = model.AssetId;
        BuildingId = model.BuildingId;
        PropertyCondition = model.PropertyCondition;
        RequestNumber = model.RequestNumber;
        Status = model.Status;
        Purpose = model.Purpose;
        PhotosPerArea = model.PhotosPerArea;
        IsPhotosRequired = model.IsPhotosRequired;
        Instructions = model.FurtherInstructions;
        RequestedAmount = !model.RequestedAmount.Equals(null) ? model.RequestedAmount : null;
        PurposeProjectName = !string.IsNullOrEmpty(model.PurposeProjectName) ? model.PurposeProjectName : null;
        BuildingComponents = model.BuildingComponents
            .GroupBy(c => c.Category)
            .SelectMany(g => g.Select((c, index) => new MaintenanceRequestBuildingComponentsModel
            {
                Id = c.Id,
                Category = g.Key,
                SubCategories = c.SubCategories
            }))
            .ToList();
        Documents = model.Documents.Select(doc => new MaintenanceRequestDocumentModel
        {
            Id = doc.Id,
            Name = doc.Name,
            Group = EnumExtensions.GetDescriptionFromValue<MaintenanceDocumentType>(doc.Group),
            Uri = doc.Uri,
            FileSize = doc.FileSize
        });

    }
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }
    public string PropertyCondition { get; set; }
    public string RequestNumber { get; set; }
    public string BuildingId { get; set; }
    public string Status { get; set; }
    public string? Purpose { get; set; }
    public int PhotosPerArea { get; set; }
    public bool? IsPhotosRequired { get; set; }
    public string? Instructions { get; set; }
    public decimal? RequestedAmount { get; set; }
    public string? PurposeProjectName { get; set; }
    public IEnumerable<MaintenanceRequestBuildingComponentsModel> BuildingComponents { get; set; }
    public IEnumerable<MaintenanceRequestDocumentModel>? Documents { get; set; }

}
