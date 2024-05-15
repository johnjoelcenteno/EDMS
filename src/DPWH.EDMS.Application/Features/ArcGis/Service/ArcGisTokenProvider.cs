using System.Text.Json;
using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Domain.Exceptions;

namespace DPWH.EDMS.Application.Features.ArcGis.Service;

public class ArcGisTokenProvider : IArcGisTokenProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ArcGisIntegrationSettings _settings;

    public ArcGisTokenProvider(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        _httpClientFactory = httpClientFactory;
        _settings = configManager.ArcGisIntegration;
    }

    public async Task<ArcGisAccessToken> GetAccessToken(CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var content = new FormUrlEncodedContent(_settings.BuildTokenRequest());

        var response = await httpClient.PostAsync(_settings.TokenUrl, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new AppException($"Failed getting access token from ArcGis: {response.ReasonPhrase}");
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var accessToken = JsonSerializer.Deserialize<ArcGisAccessToken>(
            responseContent,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        return accessToken ?? throw new AppException("Deserialization failed for ArcGIS Access Token");
    }
}