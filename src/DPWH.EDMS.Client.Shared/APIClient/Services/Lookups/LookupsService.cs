
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;

public class LookupsService : ILookupsService
{
    private readonly LookupsClient _client;

    public LookupsService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.BaseApiClientName);
        _client = new LookupsClient(httpClient);
    }

    public async Task<CommonLookup> GetLookups()
    {
        return await _client.LookupsAsync();
    }

    public async Task<AddressLookup> GetRegions()
    {
        return await _client.GetRegionsAsync();
    }

    public async Task<AddressLookup> GetProvinces(string regionCode)
    {
        return await _client.GetProvincesAsync(regionCode);
    }

    public async Task<AddressLookup> GetCityOrMunicipalities(string provinceCode)
    {
        return await _client.GetCityOrMunicipalitiesAsync(provinceCode);
    }

    public async Task<AddressLookup> GetBarangays(string cityMunicipalityCode)
    {
        return await _client.GetBarangaysAsync(cityMunicipalityCode);
    }
    public async Task<GetAgenciesResultIEnumerableBaseApiResponse> GetAgencyList()
    {
        return await _client.GetAgencyListAsync();
    }
    public async Task<GetRequestingOfficeResultIEnumerableBaseApiResponse> GetRequestingOfficeList()
    {
        return await _client.GetRequestingOfficeListAsync();
    }
    public async Task<GetLookupResultIEnumerableBaseApiResponse> GetIssuances()
    {
        return await _client.GetIssuancesAsync();
    }

    public async Task<GetLookupResultIEnumerableBaseApiResponse> GetEmployeeRecords()
    {
        return await _client.GetEmployeeRecordsAsync();
    }
    public async Task<GetLookupResultIEnumerableBaseApiResponse> GetArchives()
    {
        return await _client.GetArchivesAsync();
    }

    public async Task<GetValidIDsResultIEnumerableBaseApiResponse> GetValidIdTypes()
    {
        return await _client.GetValidIDsAsync();
    }

    public async Task<GetAuthorizationDocumentsResultIEnumerableBaseApiResponse> GetAuthorizationDocuments()
    {
        return await _client.GetAuthorizationDocumentsAsync();
    }

    public async Task<GetLookupResultIEnumerableBaseApiResponse> GetPurposes()
    {
        return await _client.GetPurposesAsync();
    }
}
