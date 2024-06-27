using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;

public class RecordRequestsService : IRecordRequestsService
{
    private readonly RecordRequestsClient _client;

    public RecordRequestsService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.BaseApiClientName);
        _client = new RecordRequestsClient(httpClient);
    }

    public async Task<CreateResponse> CreateRecordRequest(CreateRecordRequest request)
    {
        return await _client.CreateRecordRequestAsync(request);
    }

    public async Task<DataSourceResult> Query(DataSourceRequest body)
    {
        return await _client.QueryRecordRequestsAsync(body);
    }

    public async Task<DataSourceResult> QueryByEmployeeId(string employeeId, DataSourceRequest body)
    {
        return await _client.QueryRecordRequestsByEmployeeIdAsync(employeeId, body);
    }

    public async Task<RecordRequestModelBaseApiResponse> GetById(Guid id)
    {
        return await _client.GetRecordRequestAsync(id);
    }
}
