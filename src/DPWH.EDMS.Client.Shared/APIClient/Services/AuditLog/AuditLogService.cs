using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.AuditLog;

public class AuditLogService : IAuditLogService
{
    private readonly AuditLogsClient _client;

    public AuditLogService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.WebServerClientName);
        _client = new AuditLogsClient(httpClient);
    }

    public Task<DataSourceResult> AuditQuery(DataSourceRequest dataSourceRequest)
    {
        return _client.QueryAuditLogAsync(dataSourceRequest);
    }

    public async Task<GetAuditLogByEntityIdResultIEnumerableBaseApiResponse> GetAuditById(string id)
    {
        return await _client.GetByEntityIdAuditLogAsync(id);
    }
}