using System.Text.Json;
using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Application.Features.ArcGis.Service;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;

namespace DPWH.EDMS.Application.Features.ArcGis.Queries.FeatureServiceLayer;

/// <summary>
/// Query data from Feature Service / Layer.
/// For more information, see <see cref="https://developers.arcgis.com/rest/services-reference/enterprise/query-feature-service-layer-.htm"/>.
/// </summary>

public record FeatureServiceLayerQuery(string ServiceName, int LayerId, string Where) : IRequest<FeatureServiceLayerResult?>;

internal sealed class FeatureServiceLayerQueryHandler : IRequestHandler<FeatureServiceLayerQuery, FeatureServiceLayerResult?>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ArcGisIntegrationSettings _settings;
    private readonly IArcGisTokenProvider _tokenProvider;

    public FeatureServiceLayerQueryHandler(
        IHttpClientFactory httpClientFactory,
        ConfigManager configManager,
        IArcGisTokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _settings = configManager.ArcGisIntegration;
        _tokenProvider = tokenProvider;
    }

    public async Task<FeatureServiceLayerResult?> Handle(FeatureServiceLayerQuery request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenProvider.GetAccessToken(cancellationToken);
        var url = _settings.BuildQueryFeatureServiceLayerUrl(request.ServiceName, request.LayerId, request.Where, accessToken.Token);
        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync(url, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var queryResult = JsonSerializer.Deserialize<FeatureServiceLayerResult>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (queryResult?.Error is not null)
        {
            throw new AppException($"Error on feature layer service query for layer `{request.LayerId}`: {queryResult.Error}");
        }

        return queryResult;
    }
}