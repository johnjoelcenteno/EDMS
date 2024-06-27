using DPWH.EDMS.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;

public interface IRecordRequestsService
{
    Task<CreateResponse> CreateRecordRequest(CreateRecordRequest request);
    Task<DataSourceResult> Query(DataSourceRequest body);
    Task<RecordRequestModelBaseApiResponse> GetById(Guid id);
}
