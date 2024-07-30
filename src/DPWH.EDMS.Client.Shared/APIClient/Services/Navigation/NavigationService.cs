using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;

public class NavigationService : INavigationService
{
    private readonly NavigationClient _client;

    public NavigationService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.BaseApiClientName);
        _client = new NavigationClient(httpClient);
    }

    public async Task<CreateResponse> Create(CreateMenuItemModel request)
    {
        return await _client.CreateMenuItemAsync(request);
    }

    public async Task<DataSourceResult> Query(DataSourceRequest body)
    {
        return await _client.QueryMenuItemAsync(body);
    }

    public async Task<DataSourceResult> QueryByNavType(string navType, DataSourceRequest body)
    {
        return await _client.QueryMenuItemByNavTypeAsync(navType, body);
    }

    public async Task<DeleteResponse> Delete(Guid id)
    {
        return await _client.DeleteMenuItemAsync(id);
    }

    public async Task<MenuItemModelBaseApiResponse> GetById(Guid id)
    {
        return await _client.GetMenuItemAsync(id);        
    }

    public async Task<GuidNullableBaseApiResponse> Update(Guid id, UpdateMenuItemModel body)
    {
        return await _client.UpdateMenuItemAsync(id, body);
    }
}