using DPWH.EDMS.Application.Features.ArcGis;

namespace DPWH.EDMS.Application.Features.ArcGis.Commands.DeleteAllFeatures;

public class DeleteAllFeaturesResult : ArcGisApiBaseResponse
{
    public ArcGisDeleteResultItem[]? DeleteResults { get; set; }
}

public record ArcGisDeleteResultItem
{
    public int ObjectId { get; set; }
    public int UniqueId { get; set; }
    public string? GlobalId { get; set; }
    public bool Success { get; set; }

}

