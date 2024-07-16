using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Configurations;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;

public class RecordRequestSupportingFilesService : IRecordRequestSupportingFilesService
{
    private readonly RecordRequestSupportingFilesClient _client;

    public RecordRequestSupportingFilesService(IHttpClientFactory httpClientFactory, ConfigManager configManager)
    {
        var httpClient = httpClientFactory.CreateClient(configManager.BaseApiClientName);
        _client = new RecordRequestSupportingFilesClient(httpClient);
    }

    public Task<CreateResponse> Upload(FileParameter document, RecordRequestProvidedDocumentTypes? documentType, Guid? documentTypeId)
    {
        return _client.UploadSupportingFileAsync(document, documentType, documentTypeId);
    }

    public Task<CreateResponse> UploadRequestedRecord(FileParameter document, Guid? id)
    {
        return _client.UploadRequestedRecordFileAsync(document, id);
    }
}
