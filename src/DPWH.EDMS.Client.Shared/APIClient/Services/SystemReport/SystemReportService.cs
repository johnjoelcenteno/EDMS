using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.SystemReport;

public class SystemReportService : ISystemReportService
{
    private readonly SystemClient _client;
    public SystemReportService(IHttpClientFactory httpClientFactory,ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.BaseApiClientName);
        _client = new SystemClient(httpClient);
    }
    public async Task<CreateResponse> CreateSystemReport(CreateSystemLogsRequest body)
    {
        return await _client.CreateSystemLogsAsync(body);
    }

    public async Task<DeleteResponse> DeleteSystemLogs(Guid id)
    {
        return await _client.DeleteSystemLogAsync(id);
    }

    public async Task<SystemLogsResponse> GetSystemLog(Guid id)
    {
        return await _client.GetSystemLogAsync(id);
    }

    public async Task<DataSourceResult> QuerySystemLogs(DataSourceRequest body)
    {
        return await _client.QuerySystemLogsAsync(body);
    }

    public async Task<UpdateResponse> UpdateSystemLogs(Guid id, UpdateSystemLogsRequest body)
    {
        return await _client.UpdateSystemLogAsync(id, body);
    }
}
