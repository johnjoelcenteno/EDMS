using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;
using DPWH.EDMS.Client.Shared.Models;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;

public class DataLibraryService : IDataLibraryService
{

    private readonly DataLibrariesClient _client;

    public DataLibraryService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.WebServerClientName);
        _client = new DataLibrariesClient(httpClient);
    }

    public async Task<AddDataLibraryResultBaseApiResponse> AddDataLibraries(AddDataLibraryCommand request)
    {
        return await _client.AddDataLibraryAsync(request);
    }

    public async Task<DeleteDataLibraryResultBaseApiResponse> DeleteDataLibraries(Guid id)
    {
        return await _client.DeleteDataLibraryAsync(id);
    }

    public async Task<DataSourceResult> GetDataLibraries(DataSourceRequest body)
    {
        return await _client.GetDataLibrariesAsync(body);
    }

    public async Task<UpdateDataLibraryResultBaseApiResponse> UpdateDataLibraries(UpdateDataLibraryCommand request)
    {
        return await _client.UpdateDataLibraryAsync(request);
    }
}
