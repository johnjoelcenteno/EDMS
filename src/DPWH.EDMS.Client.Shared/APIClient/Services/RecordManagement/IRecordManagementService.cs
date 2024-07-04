using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RecordManagement
{
    public interface IRecordManagementService
    {
        Task<CreateResponse> Create(CreateRecordModel request);
        Task<DeleteResponse> Delete(Guid id);
        Task<RecordModelBaseApiResponse> GetById(Guid id);
        Task<DataSourceResult> Query(DataSourceRequest body);
        Task<DataSourceResult> QueryByEmployeeId(string employeeId, DataSourceRequest body);
    }
}