using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Application.Features.ArcGis.Service;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using System.Text.Json;

namespace DPWH.EDMS.Application.Features.ArcGis.Commands.DeleteAllFeatures;

public record DeleteAllFeaturesCommand(string ServiceName, int LayerId, string where) : IRequest<DeleteAllFeaturesResult?>;

internal sealed class DeleteAllFeaturesHandler : IRequestHandler<DeleteAllFeaturesCommand, DeleteAllFeaturesResult?>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ArcGisIntegrationSettings _settings;
    private readonly IArcGisTokenProvider _tokenProvider;

    public DeleteAllFeaturesHandler(
        IHttpClientFactory httpClientFactory,
        ConfigManager configManager,
        IArcGisTokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
        _settings = configManager.ArcGisIntegration;
    }

    public async Task<DeleteAllFeaturesResult?> Handle(DeleteAllFeaturesCommand request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenProvider.GetAccessToken(cancellationToken);
        var url = _settings.BuildDeleteAllFeaturesUrl(request.ServiceName, request.LayerId, accessToken.Token, request.where);
        var client = _httpClientFactory.CreateClient();

        var response = await client.PostAsync(url, null, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var deletedFeaturesResult = JsonSerializer.Deserialize<DeleteAllFeaturesResult>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (deletedFeaturesResult?.Error is not null)
        {
            throw new AppException($"Failed to delete feature layer objects: {deletedFeaturesResult.Error}");
        }

        return deletedFeaturesResult;
    }
}