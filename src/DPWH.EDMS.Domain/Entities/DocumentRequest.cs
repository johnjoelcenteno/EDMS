namespace DPWH.EDMS.Domain;

public class DocumentRequest
{
    public Guid Id { get; set; }
    public string ControlNumber { get; set; }
    public string EmployeeNumber { get; set; }
    public string EmployeeRecordsId { get; set; }
    public string ClaimedBy { get; set; }
    public string AuthorizedRepresentative { get; set; }
    public string ValidId { get; set; }
    public string SupportingDocument { get; set; }
    public string DocumentRecordsId { get; set; }
    public string DateRequested { get; set; }
    public string RequestedRecord { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset Modified { get; set; }
    public string ModifiedBy { get; set; }
}
