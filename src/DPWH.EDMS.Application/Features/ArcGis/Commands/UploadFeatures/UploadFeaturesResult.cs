using DPWH.EDMS.Application.Features.ArcGis;

namespace DPWH.EDMS.Application.Features.ArcGis.Commands.UploadFeatures;
public class UploadFeaturesResult : ArcGisApiBaseResponse
{
    public ArcGisUploadFeatureResultItem[] AddResults { get; set; }

    public class ArcGisUploadFeatureResultItem
    {
        public int ObjectId { get; set; }
        public int UniqueId { get; set; }
        public string? GlobalId { get; set; }
        public bool Success { get; set; }
        public ArcGisUploadFeatureResultErrorItem? Error { get; set; }
    }
    public record ArcGisUploadFeatureResultErrorItem
    {
        public int Code { get; set; }
        public string? Description { get; set; }
    }

}

