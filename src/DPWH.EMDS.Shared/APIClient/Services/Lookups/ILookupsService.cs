using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;

public interface ILookupsService
{
    Task<CommonLookup> GetLookups();
    Task<GetValidIDsResultIEnumerableBaseApiResponse> GetValidIdTypes();
    Task<GetSecondaryIDsResultIEnumerableBaseApiResponse> GetSecondaryIdTypes();
    Task<GetRecordTypesResultIEnumerableBaseApiResponse> GetRecordTypes();

    //Task<AddressLookup> GetRegions();
    //Task<AddressLookup> GetProvinces(string regionCode);
    ////Task<AddressLookup> GetCitiesWithoutProvince(string regionCode);
    //Task<AddressLookup> GetCities(string provinceCode);
    //Task<AddressLookup> GetBarangays(string cityCode);
    //Task<GetRequestingOfficeResultIEnumerableBaseApiResponse> GetRequestingOffices();
    //Task<GetAgenciesResultIEnumerableBaseApiResponse> GetDepartmentsQuery();
    //Task<GetBuildingComponentsResultIEnumerableBaseApiResponse> GetBuildingComponents();
}