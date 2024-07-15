using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Shared.Enums;
using UUIDNext;

namespace DPWH.EDMS.Domain.Entities;

public class RecordRequest : EntityBase
{
    private RecordRequest() { }

    public static RecordRequest Create(
       int controlNumber,
       string? employeeNumber,
       string? email,
       ClaimantTypes claimantType,
       DateTimeOffset dateRequested,
       AuthorizedRepresentative? representative,
       string purpose,
       string? remarks,
       string createdBy,
       string fullName
    )
    {        
        var entity = new RecordRequest
        {
            Id = Uuid.NewDatabaseFriendly(Database.SqlServer),
            ControlNumber = controlNumber,
            EmployeeNumber = employeeNumber,
            Email = email,
            ClaimantType = claimantType.ToString(),
            DateRequested = dateRequested,
            AuthorizedRepresentative = representative,
            Purpose = purpose,
            Remarks = remarks,
            Status = RecordRequestStates.Review.ToString(),
            RequestedRecords = [],
            FullName = fullName
        };
        entity.SetCreated(createdBy);
        return entity;
    }

    public void UpdateStatus(RecordRequestStates status, string modifiedBy)
    {
        Status = status.ToString(); 

        SetModified(modifiedBy);
    }
    public int ControlNumber { get; private set; }
    public string? EmployeeNumber { get; private set; }
    public string? Email { get; private set; }
    public string ClaimantType { get; private set; }
    public DateTimeOffset DateRequested { get; private set; }
    public AuthorizedRepresentative? AuthorizedRepresentative { get; private set; }
    public string Purpose { get; private set; }
    public string? Remarks {  get; private set; }
    public string Status { get; private set; }
    public string? FullName { get; set; }
    public virtual IList<RequestedRecord> RequestedRecords { get; set; }
    public virtual IList<RecordRequestDocument>? Files { get; set; }
}
