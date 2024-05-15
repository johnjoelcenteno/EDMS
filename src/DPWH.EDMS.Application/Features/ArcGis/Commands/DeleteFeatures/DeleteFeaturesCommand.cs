using System.Text.Json;
using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Application.Features.ArcGis.Service;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;

namespace DPWH.EDMS.Application.Features.ArcGis.Commands.DeleteFeatures;

/// <summary>
/// Deletes feature(s) in a feature layer or table.
/// For more information, see <see cref="https://developers.arcgis.com/rest/services-reference/enterprise/delete-features.htm"/>.
/// </summary>
public record DeleteFeaturesCommand(string ServiceName, int LayerId, int[] ObjectIds) : IRequest<DeleteFeaturesResult?>;

internal sealed class DeleteFeaturesHandler : IRequestHandler<DeleteFeaturesCommand, DeleteFeaturesResult?>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ArcGisIntegrationSettings _settings;
    private readonly IArcGisTokenProvider _tokenProvider;

    public DeleteFeaturesHandler(
        IHttpClientFactory httpClientFactory,
        ConfigManager configManager,
        IArcGisTokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
        _settings = configManager.ArcGisIntegration;
    }

    public async Task<DeleteFeaturesResult?> Handle(DeleteFeaturesCommand request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenProvider.GetAccessToken(cancellationToken);
        var url = _settings.BuildDeleteFeaturesUrl(request.ServiceName, request.LayerId, request.ObjectIds, accessToken.Token);
        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsync(url, null, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var deletedFeaturesResult = JsonSerializer.Deserialize<DeleteFeaturesResult>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (deletedFeaturesResult?.Error is not null)
        {
            throw new AppException($"Failed to delete feature layer objects: {deletedFeaturesResult.Error}");
        }

        return deletedFeaturesResult;
    }
}