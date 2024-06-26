using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.DpwhIntegrations;

public class DpwhIntegrationsService : IDpwhIntegrationsService
{
    private readonly DpwhIntegrationsClient _client; 
    public DpwhIntegrationsService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.BaseApiClientName);
        _client = new DpwhIntegrationsClient(httpClient); 
    } 
    public async Task<EmployeeBaseApiResponse> GetByEmployeeId(string id)
    {
        return await _client.GetEmployeeByIdAsync(id);
    }
}

