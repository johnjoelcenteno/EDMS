

using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.Reports;

public interface IReportsService
{
    Task<DataSourceResult> QueryUser(DataSourceRequest request);
}
