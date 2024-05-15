using DPWH.EDMS.Application.Features.ArcGis;

namespace DPWH.EDMS.Application.Features.ArcGis.Commands.UpdateFeatures;

public class UpdateFeaturesResult : ArcGisApiBaseResponse
{
    public ArcGisUpdateFeatureResultItem[] UpdateResults { get; set; }
}

public class ArcGisUpdateFeatureResultItem
{
    public int ObjectId { get; set; }
    public int UniqueId { get; set; }
    public string? GlobalId { get; set; }
    public bool Success { get; set; }
    public ArcGisUpdateFeatureResultErrorItem? Error { get; set; }
}

public record ArcGisUpdateFeatureResultErrorItem
{
    public int Code { get; set; }
    public string? Description { get; set; }
}