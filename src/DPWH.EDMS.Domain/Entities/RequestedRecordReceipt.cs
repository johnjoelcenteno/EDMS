using DPWH.EDMS.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;
/// <summary>
/// This represents uploaded transmittal receipt when claiming a record request.
/// </summary>

public class RequestedRecordReceipt : EntityBase
{
    private RequestedRecordReceipt(Guid? recordRequestId, string? filename, string? office, long? fileSize, string? uri, DateTimeOffset? dateReceived, DateTimeOffset? timeReceived, string createdBy)
    {
        RecordRequestId = recordRequestId;
        Filename = filename;
        Office = office;
        FileSize = fileSize;
        Uri = uri;
        DateReceived = dateReceived;
        TimeReceived = timeReceived;

        SetCreated(createdBy);
    }

    public static RequestedRecordReceipt Create(Guid? recordRequestId, string? filename, string? office, long? fileSize, string? uri, DateTimeOffset? dateReceived, DateTimeOffset? timeReceived, string createdBy)
    {
        return new RequestedRecordReceipt(recordRequestId, filename, office, fileSize, uri, dateReceived, timeReceived, createdBy);
    }

    [ForeignKey("RecordRequestId")]
    public Guid? RecordRequestId { get; set; }
    public string? Filename { get; set; }
    public string? Office { get; set; }
    public long? FileSize { get; set; }
    public string? Uri { get; set; }
    public DateTimeOffset? DateReceived { get; set; }
    public DateTimeOffset? TimeReceived { get; set; }
}
