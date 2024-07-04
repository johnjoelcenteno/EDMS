﻿using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;

public class RequestManagementService : IRequestManagementService
{
    private readonly Requests_ManagementClient _client;

    public RequestManagementService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.BaseApiClientName);
        _client = new Requests_ManagementClient(httpClient);
    }

    public async Task<RecordRequestModelBaseApiResponse> GetById(Guid id)
    {
        return await _client.GetRecordRequestAsync(id);
    }

    public async Task<DataSourceResult> Query(DataSourceRequest body)
    {
        return await _client.QueryRecordRequestsAsync(body);
    }

    public async Task<DataSourceResult> QueryByEmployeeId(string employeeId, DataSourceRequest body)
    {
        return await _client.QueryRecordRequestsByEmployeeIdAsync(employeeId, body);
    }

    public async Task<DataSourceResult> QueryByStatus(string status, DataSourceRequest body)
    {
        return await _client.QueryRecordRequestsByStatusAsync(status, body);
    }

    public async Task<CreateResponse> Create(CreateRecordRequest req)
    {
        return await _client.CreateRecordRequestAsync(req);
    }

    public async Task<EmployeeBaseApiResponse> Update(string employeeId)
    {
        return await _client.Update_document_requestAsync(employeeId);
    }

    public async Task<DeleteResponse> Delete(Guid id)
    {
        return await _client.DeleteRecordRequestAsync(id);
    }
}