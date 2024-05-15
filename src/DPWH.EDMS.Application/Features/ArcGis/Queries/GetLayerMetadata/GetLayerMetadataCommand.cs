using System.Text.Json;
using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Application.Features.ArcGis.Service;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;

namespace DPWH.EDMS.Application.Features.ArcGis.Queries.GetLayerMetadata;

/// <summary>
/// Retrieves feature layer metadata.
/// For more information, see <see cref="https://developers.arcgis.com/rest/services-reference/enterprise/feature-layer.htm"/>.
/// </summary>
public record GetLayerMetadataCommand(string ServiceName, int LayerId) : IRequest<GetLayerMetadataResult?>;

internal sealed class GetLayerMetadataHandler : IRequestHandler<GetLayerMetadataCommand, GetLayerMetadataResult?>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IArcGisTokenProvider _tokenProvider;
    private readonly ArcGisIntegrationSettings _settings;

    public GetLayerMetadataHandler(
        IHttpClientFactory httpClientFactory,
        ConfigManager configManager,
        IArcGisTokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
        _settings = configManager.ArcGisIntegration;
    }

    public async Task<GetLayerMetadataResult?> Handle(GetLayerMetadataCommand request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenProvider.GetAccessToken(cancellationToken);
        var url = _settings.BuildGetLayerMetadataUrl(request.ServiceName, request.LayerId, accessToken.Token);
        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync(url, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var metadata = JsonSerializer.Deserialize<GetLayerMetadataResult>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (metadata?.Error is not null)
        {
            throw new AppException($"Failed to retrieve layer `{request.LayerId}` metadata: {@metadata.Error}");
        }

        return metadata;
    }
}