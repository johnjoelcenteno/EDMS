using DPWH.EDMS.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.SystemReport;

public interface ISystemReportService
{
    Task<CreateResponse> CreateSystemReport(CreateSystemLogsRequest body);
    Task<SystemLogsResponse> GetSystemLog(Guid id);
    Task<DataSourceResult> QuerySystemLogs(DataSourceRequest body);
    Task<UpdateResponse> UpdateSystemLogs(Guid id,UpdateSystemLogsRequest body);

    Task<DeleteResponse> DeleteSystemLogs(Guid id);

}
