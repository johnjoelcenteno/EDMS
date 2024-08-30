using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Signatories
{
    public interface ISignatoryManagementService
    {
        Task<GuidBaseApiResponse> Create(CreateSignatoryModel body);
        Task<DataSourceResult> Query(DataSourceRequest body);
        Task<GuidNullableBaseApiResponse> UpdateSignatoriesAsync(Guid id, UpdateSignatoryModel body);
        Task<GuidNullableBaseApiResponse> DeleteSignatoriesAsync(Guid id);
        Task<QuerySignatoryModelBaseApiResponse> GetSignatoryByEmployeeId(string employeeId);
    }
} 