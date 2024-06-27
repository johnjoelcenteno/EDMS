using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.Models;

public class UploadSupportingFileRequestModel
{
    public required FileParameter document { get; set; }
    public required RecordRequestProvidedDocumentTypes? documentType { get; set; }
    public required Guid? documentTypeId { get; set; }
}
