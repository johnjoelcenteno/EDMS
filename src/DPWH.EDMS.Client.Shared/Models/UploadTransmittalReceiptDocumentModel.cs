using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.Models;

public class UploadTransmittalReceiptDocumentModel
{
    public required FileParameter Document { get; set; }
    public required Guid? RecordRequestId { get; set; }
}
