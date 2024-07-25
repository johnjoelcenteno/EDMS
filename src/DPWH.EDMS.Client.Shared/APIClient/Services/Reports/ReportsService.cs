using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Reports;

public class ReportsService : IReportsService
{
    private readonly ReportsClient _client;

    public ReportsService(IHttpClientFactory httpClientFactory,ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.BaseApiClientName);
        _client = new ReportsClient(httpClient);
    }
    public async Task<DataSourceResult> QueryUser(DataSourceRequest request)
    {
        return await _client.UsersAsync(request);
    }
}
