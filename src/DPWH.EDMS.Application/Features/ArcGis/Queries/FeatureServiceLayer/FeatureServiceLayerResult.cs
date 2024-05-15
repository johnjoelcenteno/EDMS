using DPWH.EDMS.Application.Features.ArcGis;

namespace DPWH.EDMS.Application.Features.ArcGis.Queries.FeatureServiceLayer;

public class FeatureServiceLayerResult : ArcGisApiBaseResponse
{
    public ArcGisGetFeatureServiceLayer[]? Features { get; set; }
}

public class ArcGisGetFeatureServiceLayer
{
    public Dictionary<string, object>? Attributes { get; set; }
    public ArcGisGetGeometry? Geometry { get; set; }
}

public class ArcGisGetGeometry
{
    public double X { get; set; }
    public double Y { get; set; }
    public ArcGisGetSpatialReference? SpatialReference { get; set; }

}
public class ArcGisGetSpatialReference
{
    public int Wkid { get; set; }
    public int LatestWkid { get; set; }
    public int VcsWkid { get; set; }
    public int LatestVcsWkid { get; set; }
}