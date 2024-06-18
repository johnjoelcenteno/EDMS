using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
