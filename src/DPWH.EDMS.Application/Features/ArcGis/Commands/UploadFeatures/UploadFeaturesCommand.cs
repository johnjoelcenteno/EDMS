using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Application.Features.ArcGis.Service;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using System.Text.Json;

namespace DPWH.EDMS.Application.Features.ArcGis.Commands.UploadFeatures;

/// <summary>
/// Upload feature on feature layer or table.
/// For more information, see <see cref="https://developers.arcgis.com/rest/services-reference/enterprise/add-features.htm"/>
/// </summary>
public record UploadFeaturesCommand : IRequest<UploadFeaturesResult?>
{
    public string? ServiceName { get; set; }
    public int LayerId { get; set; }
    public ArcGisUploadFeatureServiceLayer[]? Features { get; set; }

    public record ArcGisUploadFeatureServiceLayer
    {
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
        public ArcGisUploadGeometry Geometry { get; set; }
    }

    public class ArcGisUploadGeometry
    {
        public double? X { get; set; }
        public double? Y { get; set; }
        public ArcGisUploadSpatialReference? SpatialReference { get; set; }

    }
    public class ArcGisUploadSpatialReference
    {
        public int Wkid { get; set; }
    }
}

internal sealed class UploadFeaturesHandler : IRequestHandler<UploadFeaturesCommand, UploadFeaturesResult?>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ArcGisIntegrationSettings _setting;
    private readonly IArcGisTokenProvider _tokenProvider;

    public UploadFeaturesHandler(
        IHttpClientFactory httpClientFactory,
        ConfigManager configManager,
        IArcGisTokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
        _setting = configManager.ArcGisIntegration;
    }

    public async Task<UploadFeaturesResult?> Handle(UploadFeaturesCommand request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenProvider.GetAccessToken(cancellationToken);
        var serializedFeatures = JsonSerializer.Serialize(
            request.Features, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true });

        var parameter = _setting.BuildUpdateOrUploadFeatureRequestContent(serializedFeatures, accessToken.Token);
        var httpContent = new FormUrlEncodedContent(parameter);
        var url = _setting.BuildUploadFeaturesUrl(request.ServiceName, request.LayerId);
        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsync(url, httpContent, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var uploadFeaturesResult = JsonSerializer.Deserialize<UploadFeaturesResult>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (uploadFeaturesResult?.Error is not null)
        {
            throw new AppException($"Failed to upload feature layer: {uploadFeaturesResult.Error}");
        }

        return uploadFeaturesResult;
    }
}