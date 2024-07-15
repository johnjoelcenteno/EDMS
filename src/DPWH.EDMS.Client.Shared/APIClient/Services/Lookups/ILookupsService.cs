using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Lookups
{
    public interface ILookupsService
    {
        Task<GetAgenciesResultIEnumerableBaseApiResponse> GetAgencyList();
        Task<GetLookupResultIEnumerableBaseApiResponse> GetPersonalRecords();
        Task<GetAuthorizationDocumentsResultIEnumerableBaseApiResponse> GetAuthorizationDocuments();
        Task<AddressLookup> GetBarangays(string cityMunicipalityCode);
        Task<AddressLookup> GetCityOrMunicipalities(string provinceCode);
        Task<GetLookupResultIEnumerableBaseApiResponse> GetEmployeeRecords();
        Task<GetLookupResultIEnumerableBaseApiResponse> GetIssuances();
        Task<CommonLookup> GetLookups();
        Task<AddressLookup> GetProvinces(string regionCode);
        Task<GetLookupResultIEnumerableBaseApiResponse> GetPurposes();
        Task<AddressLookup> GetRegions();
        Task<GetRequestingOfficeResultIEnumerableBaseApiResponse> GetRequestingOfficeList();
        Task<GetValidIDsResultIEnumerableBaseApiResponse> GetValidIdTypes();
    }
}