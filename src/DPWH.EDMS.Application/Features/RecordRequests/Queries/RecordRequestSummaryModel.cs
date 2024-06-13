namespace DPWH.EDMS.Application.Features.RecordRequests.Queries;
public record RecordRequestSummaryModel
{
    public Guid Id { get; set; }
    public string ControlNumber { get; set; }
    public string ClaimantType { get; set; }
    public DateTimeOffset DateRequested { get; set; }        
    public string Purpose { get; set; }
    public string Status { get; set; }    
    public IEnumerable<RequestedRecordModel> RequestedRecords { get; set; } = [];
}
