using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Signatories;
public class SignatoryManagementService : ISignatoryManagementService
{
    private readonly SignatoriesClient _client;

    public SignatoryManagementService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.WebServerClientName);
        _client = new SignatoriesClient(httpClient);
    }

    public async Task<GuidBaseApiResponse> Create(CreateSignatoryModel body)
    {
        return await _client.Creates_new_signatoryAsync(body);
    }

    public async Task<DataSourceResult> Query(DataSourceRequest body)
    {
        return await _client.Query_signatoriesAsync(body);
    }

    public async Task<GuidNullableBaseApiResponse> UpdateSignatoriesAsync(Guid id, UpdateSignatoryModel body)
    {
        return await _client.Updates_signatoryAsync(id, body);
    }

    public async Task<GuidNullableBaseApiResponse> DeleteSignatoriesAsync(Guid id)
    {
        return await _client.Delete_signatoryAsync(id);
    }

}