using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement
{
    public interface IRequestManagementService
    {
        Task<CreateResponse> Create(CreateRecordRequest req);
        Task<DeleteResponse> Delete(Guid id);
        Task<RecordRequestModelBaseApiResponse> GetById(Guid id);
        Task<DataSourceResult> Query(DataSourceRequest body);
        Task<DataSourceResult> QueryByEmployeeId(string employeeId, DataSourceRequest body);
        Task<DataSourceResult> QueryByStatus(string status, DataSourceRequest body);
        Task<UpdateResponseBaseApiResponse> UpdateStatus(UpdateRecordRequestStatus req);
    }
}