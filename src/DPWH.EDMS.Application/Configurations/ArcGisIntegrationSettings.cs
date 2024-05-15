using System.Text;

namespace DPWH.EDMS.Application.Configurations;

public class ArcGisIntegrationSettings
{
    public string? TokenUrl { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Referer { get; set; }
    public string? Client { get; set; }
    public string? Expiration { get; set; }
    public string? ResponseFormat { get; set; }
    public string? Host { get; set; }
    public string? OrgId { get; set; }
    public string? ServiceName { get; set; }
    public int LayerId { get; set; }

    public IEnumerable<KeyValuePair<string, string?>> BuildTokenRequest()
    {
        return new[]
        {
            new KeyValuePair<string, string?>("username", UserName),
            new KeyValuePair<string, string?>("password", Password),
            new KeyValuePair<string, string?>("f", ResponseFormat),
            new KeyValuePair<string, string?>("referer", Referer),
            new KeyValuePair<string, string?>("client", Client),
            new KeyValuePair<string, string?>("expiration", Expiration)
        };
    }

    public string BuildGetLayerMetadataUrl(string serviceName, int layerId, string? token)
    {
        return new StringBuilder("https://")
            .Append(Host)
            .Append(".arcgis.com/")
            .Append(OrgId)
            .Append("/ArcGIS/rest/services/")
            .Append(serviceName)
            .Append("/FeatureServer/")
            .Append(layerId)
            .Append("?f=")
            .Append(ResponseFormat)
            .Append("&token=")
            .Append(token)
            .ToString();
    }

    public string BuildUploadFeaturesUrl(string? serviceName, int layerId)
    {
        return new StringBuilder("https://")
            .Append(Host)
            .Append(".arcgis.com/")
            .Append(OrgId)
            .Append("/ArcGIS/rest/services/")
            .Append(serviceName)
            .Append("/FeatureServer/")
            .Append(LayerId)
            .Append("/addFeatures")
            .ToString();
    }

    public string BuildDeleteFeaturesUrl(string serviceName, int layerId, int[] objectIds, string? token)
    {
        return new StringBuilder("https://")
            .Append(Host)
            .Append(".arcgis.com/")
            .Append(OrgId)
            .Append("/ArcGIS/rest/services/")
            .Append(serviceName)
            .Append("/FeatureServer/")
            .Append(layerId)
            .Append("/deleteFeatures")
            .Append("?f=")
            .Append(ResponseFormat)
            .Append("&objectIds=")
            .Append(string.Join(',', objectIds))
            .Append("&token=")
            .Append(token)
            .ToString();
    }
    public string BuildDeleteAllFeaturesUrl(string serviceName, int layerId, string? token, string where)
    {
        return new StringBuilder("https://")
            .Append(Host)
            .Append(".arcgis.com/")
            .Append(OrgId)
            .Append("/ArcGIS/rest/services/")
            .Append(serviceName)
            .Append("/FeatureServer/")
            .Append(layerId)
            .Append("/deleteFeatures")
            .Append("?f=")
            .Append(ResponseFormat)
            .Append("&token=")
            .Append(token)
            .Append("&where=")
            .Append(where)
            .ToString();
    }

    public string BuildQueryFeatureServiceLayerUrl(string serviceName, int layerId, string where, string? token)
    {
        return new StringBuilder("https://")
            .Append(Host)
            .Append(".arcgis.com/")
            .Append(OrgId)
            .Append("/ArcGIS/rest/services/")
            .Append(serviceName)
            .Append("/FeatureServer/")
            .Append(layerId)
            .Append("/query")
            .Append("?f=")
            .Append(ResponseFormat)
            .Append("&where=")
            .Append(where)
            .Append("&outFields=*")
            .Append("&token=")
            .Append(token)
            .ToString();
    }

    public string BuildUpdateFeatureServiceLayerUrl(string? serviceName, int layerId)
    {
        return new StringBuilder("https://")
            .Append(Host)
            .Append(".arcgis.com/")
            .Append(OrgId)
            .Append("/ArcGIS/rest/services/")
            .Append(serviceName)
            .Append("/FeatureServer/")
            .Append(layerId)
            .Append("/updateFeatures")
            .ToString();
    }

    public IEnumerable<KeyValuePair<string, string?>> BuildUpdateOrUploadFeatureRequestContent(string? features, string? token)
    {
        return new[]
        {
            new KeyValuePair<string, string?>("f", ResponseFormat),
            new KeyValuePair<string, string?>("features", features),
            new KeyValuePair<string, string?>("token", token)
        };
    }
}


