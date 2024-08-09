using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.Models;

public class UploadRequestedRecordDocumentModel
{
    public required FileParameter Document { get; set; }
    public required Guid? Id { get; set; }
    public string? DocumentType { get; set; }
}
