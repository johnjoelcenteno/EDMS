using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain;

public class DocumentRequest : EntityBase
{
    public Guid Id { get; set; }
    public string EmployeeNumber { get; set; }
    public string ControlNumber { get; set; }
    public Guid EmployeeRecordsId { get; set; }
    public string ClaimedBy { get; set; }
    public string AuthorizedRepresentative { get; set; }
    public string ValidId { get; set; }
    public string SupportingDocument { get; set; }
    public Guid DocumentRecordsId { get; set; }
    public DateTimeOffset DateRequested { get; set; }
    public string RequestedRecord { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
}
