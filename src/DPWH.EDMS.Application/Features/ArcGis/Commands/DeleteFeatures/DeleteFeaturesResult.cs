namespace DPWH.EDMS.Application.Features.ArcGis.Commands.DeleteFeatures;

public class DeleteFeaturesResult : ArcGisApiBaseResponse
{
    public ArcGisDeleteResultItem[]? DeleteResults { get; set; }
    public bool Success { get; set; }
}

public record ArcGisDeleteResultItem
{
    public int ObjectId { get; set; }
    public int UniqueId { get; set; }
    public string? GlobalId { get; set; }
    public bool Success { get; set; }
    public ArcGisDeleteResultErrorItem? Error { get; set; }
}

public record ArcGisDeleteResultErrorItem
{
    public int Code { get; set; }
    public string? Description { get; set; }
}