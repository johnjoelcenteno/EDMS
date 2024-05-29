using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;

public class LicensesService : ILicensesService
{
    private readonly LicensesClient _client;

    public LicensesService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.BaseApiClientName);
        _client = new LicensesClient(httpClient);
    }

    public Task<GetLicenseStatusResultBaseApiResponse> GetLicenseStatus()
    {
        return _client.GetLicenseStatusAsync();
    }
}
