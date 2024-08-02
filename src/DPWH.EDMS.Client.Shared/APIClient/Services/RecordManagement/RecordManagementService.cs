using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RecordManagement;

public class RecordManagementService : IRecordManagementService
{
    private readonly Records_ManagementClient _client;

    public RecordManagementService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {        
        var httpClient = httpClientFactory.CreateClient(configManager.WebServerClientName);        
        _client = new Records_ManagementClient(httpClient);        
    }

    public async Task<CreateResponse> Create(CreateRecordModel request)
    {
        return await _client.CreateRecordAsync(request);
    }

    public async Task<DeleteResponse> Delete(Guid id)
    {
        return await _client.DeleteRecordAsync(id);
    }

    public async Task<DataSourceResult> Query(DataSourceRequest body)
    {
        return await _client.QueryRecordsAsync(body);
    }

    public async Task<DataSourceResult> QueryByEmployeeId(string employeeId, DataSourceRequest body)
    {
        return await _client.QueryRecordsByEmployeeIdAsync(employeeId, body);
    }

    public async Task<RecordModelBaseApiResponse> GetById(Guid id)
    {
        return await _client.GetRecordByIdAsync(id);
    }
}
