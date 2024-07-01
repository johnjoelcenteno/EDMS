using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.Models;

public class UploadRecordRequestDocumentModel
{
    public required FileParameter Document { get; set; }
    public required RecordRequestProvidedDocumentTypes? DocumentType { get; set; }
    public required Guid? DocumentTypeId { get; set; }
}
