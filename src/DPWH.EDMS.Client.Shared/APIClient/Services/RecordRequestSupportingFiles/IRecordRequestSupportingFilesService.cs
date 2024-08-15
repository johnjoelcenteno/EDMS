using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles
{
    public interface IRecordRequestSupportingFilesService
    {
        Task<CreateResponse> Upload(FileParameter document, RecordRequestProvidedDocumentTypes? documentType, Guid? documentTypeId);
        Task<CreateResponse> UploadRequestedRecord(FileParameter document, Guid? id, string DocumentType);
        Task<CreateResponse> UploadTransmittalReceipt(System.DateTimeOffset dateReceived, System.DateTimeOffset timeReceived, FileParameter document, Guid? id);
        Task<UpdateResponseBaseApiResponse> UpdateRecordStatus(UpdateRecordsRequestDocumentStatus req);
        Task<GetTransmittalReceiptModelBaseApiResponse> GetTransmittalReceipt(Guid id);
    }
}