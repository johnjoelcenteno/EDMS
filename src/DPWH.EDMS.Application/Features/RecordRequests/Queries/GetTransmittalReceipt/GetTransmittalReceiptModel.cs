namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetTransmittalReceipt;

public class GetTransmittalReceiptModel
{
    public string? Filename {  get; set; }
    public string? Uri { get; set; }
    public string? Office { get; set; }
    public DateTimeOffset? DateReceived { get; set; }
    public DateTimeOffset? TimeReceived { get; set; }
}
