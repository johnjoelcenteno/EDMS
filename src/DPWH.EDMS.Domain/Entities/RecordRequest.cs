using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Shared.Enums;

namespace DPWH.EDMS.Domain.Entities;

public class RecordRequest : EntityBase
{
    private RecordRequest() { }

    public static RecordRequest Create(
       int controlNumber,
       string employeeNumber,
       ClaimantTypes claimantType,
       DateTimeOffset dateRequested,
       AuthorizedRepresentative? representative,
       string purpose,
       string createdBy,
       string fullName
    )
    {
        var entity = new RecordRequest
        {
            Id = Guid.NewGuid(),
            ControlNumber = controlNumber,
            EmployeeNumber = employeeNumber,
            ClaimantType = claimantType.ToString(),
            DateRequested = dateRequested,
            AuthorizedRepresentative = representative,
            Purpose = purpose,
            Status = RecordRequestStates.Review.ToString(),
            RequestedRecords = [],
            FullName = fullName
        };
        entity.SetCreated(createdBy);
        return entity;
    }
    public int ControlNumber { get; private set; }
    public string EmployeeNumber { get; private set; }
    public string ClaimantType { get; private set; }
    public DateTimeOffset DateRequested { get; private set; }
    public AuthorizedRepresentative? AuthorizedRepresentative { get; private set; }
    public string Purpose { get; private set; }
    public string Status { get; private set; }
    public string? FullName { get; set; }
    public virtual IList<RequestedRecord> RequestedRecords { get; set; }
    public virtual IList<RecordRequestDocument>? Files { get; set; }
}
