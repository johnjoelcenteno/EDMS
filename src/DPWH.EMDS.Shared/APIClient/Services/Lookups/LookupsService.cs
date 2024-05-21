
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;

internal class LookupsService : ILookupsService
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
    
    ////public async Task<AddressLookup> GetRegions()
    ////{
    ////    return await _client.GetRegionsAsync();
    ////}
    
    ////public async Task<AddressLookup> GetProvinces(string regionCode)
    ////{
    ////    return await _client.GetProvincesAsync(regionCode);
    ////}
    
    //////public async Task<AddressLookup> GetCitiesWithoutProvince(string regionCode)
    //////{
    //////    return await _client.GetCityOrMunicipalitiesWithoutProvinceAsync(regionCode);
    //////}

    ////public async Task<AddressLookup> GetCities(string provinceCode)
    ////{
    ////    return await _client.GetCityOrMunicipalitiesAsync(provinceCode);
    ////}
    
    ////public async Task<AddressLookup> GetBarangays(string cityCode)
    ////{
    ////    return await _client.GetBarangaysAsync(cityCode);
    ////}

    ////public async Task<GetRequestingOfficeResultIEnumerableBaseApiResponse> GetRequestingOffices()
    ////{
    ////    return await _client.GetRequestingOfficeListAsync();
    ////}

    ////public async Task<GetAgenciesResultIEnumerableBaseApiResponse> GetDepartmentsQuery()
    ////{
    ////    return await _client.GetAgencyListAsync();
    ////}
    
    ////public async Task<GetBuildingComponentsResultIEnumerableBaseApiResponse> GetBuildingComponents()
    ////{
    ////    return await _client.GetBuildingComponentsAsync();
    ////}
}
