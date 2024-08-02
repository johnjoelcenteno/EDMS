using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RecordTypes;

public class RecordTypesService : IRecordTypesService
{
    private readonly RecordTypesClient _client;

    public RecordTypesService(IHttpClientFactory httpClientFactory,ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.WebServerClientName);
        _client = new RecordTypesClient(httpClient);
    }

    public async Task<GuidBaseApiResponse> CreateRecordTypesAsync(CreateRecordTypeModel model)
    {
        return await _client.Create_record_typeAsync(model);
    }

    public async Task<DeleteResponse> DeleteRecordTypesAsync(Guid id)
    {
        return await _client.DeleteRecordTypeAsync(id);
    }

    public async Task<QueryRecordTypesModelListBaseApiResponse> QueryByCategoryRecordTypesAsync(string category)
    {
        return await _client.Query_record_types_by_categoryAsync(category);
    }

    public async Task<QueryRecordTypesModelBaseApiResponse> QueryByIdRecordTypesAsync(Guid id)
    {
        return await _client.Query_record_types_by_idAsync(id);
    }

    public async Task<DataSourceResult> QueryRecordTypesAsync(DataSourceRequest request)
    {
        return await _client.Query_record_typeAsync(request);
    }

    public async Task<GuidNullableBaseApiResponse> UpdateRecordTypesAsync(Guid id, UpdateRecordTypeModel model)
    {
        return await _client.UpdateRecordTypeAsync(id, model);
    }
}

