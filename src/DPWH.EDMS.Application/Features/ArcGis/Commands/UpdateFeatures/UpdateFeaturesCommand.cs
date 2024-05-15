using System.Text.Json;
using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Application.Features.ArcGis.Service;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;

namespace DPWH.EDMS.Application.Features.ArcGis.Commands.UpdateFeatures;

/// <summary>
/// Updates feature on feature layer or table.
/// For more information, see <see cref="https://developers.arcgis.com/rest/services-reference/enterprise/update-features.htm"/>
/// </summary>
public record UpdateFeaturesCommand : IRequest<UpdateFeaturesResult?>
{
    public string? ServiceName { get; set; }
    public int LayerId { get; set; }
    public ArcGisUpdateFeatureServiceLayer[]? Features { get; set; }

    public record ArcGisUpdateFeatureServiceLayer
    {
        public Dictionary<string, object>? Attributes { get; set; }
        public ArcGisUpdateGeometry? ArcGisGeometry { get; set; }
    }
    public class ArcGisUpdateGeometry
    {
        public double? X { get; set; }
        public double? Y { get; set; }
        public ArcGisUpdateSpatialReference? ArcGisSpatialReference { get; set; }
    }
    public class ArcGisUpdateSpatialReference
    {
        public int Wkid { get; set; }
    }
}

internal sealed class UpdateFeaturesHandler : IRequestHandler<UpdateFeaturesCommand, UpdateFeaturesResult?>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ArcGisIntegrationSettings _settings;
    private readonly IArcGisTokenProvider _tokenProvider;

    public UpdateFeaturesHandler(
        IHttpClientFactory httpClientFactory,
        ConfigManager configManager,
        IArcGisTokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
        _settings = configManager.ArcGisIntegration;
    }

    public async Task<UpdateFeaturesResult?> Handle(UpdateFeaturesCommand request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenProvider.GetAccessToken(cancellationToken);
        var serializedFeatures = JsonSerializer.Serialize(
            request.Features,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true });

        var parameters = _settings.BuildUpdateOrUploadFeatureRequestContent(serializedFeatures, accessToken.Token);
        var httpContent = new FormUrlEncodedContent(parameters);

        var url = _settings.BuildUpdateFeatureServiceLayerUrl(request.ServiceName, request.LayerId);
        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsync(url, httpContent, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var updateFeaturesResult = JsonSerializer.Deserialize<UpdateFeaturesResult>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (updateFeaturesResult?.Error is not null)
        {
            throw new AppException($"Failed to update feature layer: {updateFeaturesResult.Error}");
        }

        return updateFeaturesResult;
    }
}