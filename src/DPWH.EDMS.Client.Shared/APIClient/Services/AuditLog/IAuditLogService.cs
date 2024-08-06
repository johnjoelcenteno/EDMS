using DPWH.EDMS.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.AuditLog;

public interface IAuditLogService
{
    Task<DataSourceResult> AuditQuery(DataSourceRequest dataSourceRequest);
    Task<GetAuditLogByEntityIdResultIEnumerableBaseApiResponse> GetAuditById(string id);
}