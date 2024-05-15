using DPWH.EDMS.Application.Features.Assets.Queries;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetAssetByBuildingId;

public record GetAssetByBuildingIdWithBuildingComponentResult
{
    public GetAssetByBuildingIdWithBuildingComponentResult(AssetModel asset, IEnumerable<string>? requestIds)
    {
        AssetId = asset.Id;
        BuildingId = asset.BuildingId;
        StructureType = asset.ConstructionType;
        ImplementingOffice = asset.ImplementingOffice;
        Group = asset.Group;
        Agency = asset.Agency;
        AttachedAgency = asset.AttachedAgency;
        PropertyName = asset.Name;
        Region = asset.Region;
        Province = asset.Province;
        CityOrMunicipality = asset.CityOrMunicipality;
        Barangay = asset.Barangay;
        Street = asset.StreetAddress;
        PropertyCondition = asset.PropertyStatus;
        Longitude = asset.Longitude;
        Latitude = asset.Latitude;

        RequestIds = requestIds;
    }
    public Guid AssetId { get; set; }
    public string BuildingId { get; set; }
    public string StructureType { get; set; }
    public string? ImplementingOffice { get; set; }
    public string Group { get; set; }
    public string? Agency { get; set; }
    public string? AttachedAgency { get; set; }
    public string? PropertyName { get; set; }
    public string? Region { get; set; }
    public string? Province { get; set; }
    public string? CityOrMunicipality { get; set; }
    public string? Barangay { get; set; }
    public string? Street { get; set; }
    public string? PropertyCondition { get; set; }
    public LongLatFormat? Longitude { get; set; }
    public LongLatFormat? Latitude { get; set; }
    public IEnumerable<string>? RequestIds { get; set; }

}
