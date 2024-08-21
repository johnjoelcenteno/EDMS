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
       string? otherPurpose,
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
            OtherPurpose = otherPurpose,    
            Remarks = remarks,
            Status = RecordRequestStates.Submitted.ToString(),
            RequestedRecords = [],
            FullName = fullName,
            HRMDRequestStatus = OfficeRequestedRecordStatus.NA.ToString(),
            RMDRequestStatus = OfficeRequestedRecordStatus.NA.ToString(),
        };
        entity.SetCreated(createdBy);
        return entity;
    }
    public void OnCreateOfficeStatus(OfficeRequestedRecordStatus status, Offices? office)
    {
        if (office == null)
            return;

        var statusProperty = office switch
        {
            Offices.RMD => nameof(RMDRequestStatus),
            Offices.HRMD => nameof(HRMDRequestStatus),
            _ => null
        };

        if (statusProperty != null)
        {
            GetType().GetProperty(statusProperty)?.SetValue(this, status.ToString());
        }

    }

    public void UpdateStatus(RecordRequestStates status, string modifiedBy)
    {
        Status = status.ToString(); 

        SetModified(modifiedBy);
    }
    
    public void UpdateOfficeStatus(OfficeRequestedRecordStatus status, string modifiedBy, Offices? office)
    {
        if (office == null)
            return;

        var statusProperty = office switch
        {
            Offices.RMD => nameof(RMDRequestStatus),
            Offices.HRMD => nameof(HRMDRequestStatus),
            _ => null
        };

        if (statusProperty != null)
        {
            GetType().GetProperty(statusProperty)?.SetValue(this, status.ToString());
        }

    }

    public int ControlNumber { get; private set; }
    public string? EmployeeNumber { get; private set; }
    public string? Email { get; private set; }
    public string ClaimantType { get; private set; }
    public DateTimeOffset DateRequested { get; private set; }
    public AuthorizedRepresentative? AuthorizedRepresentative { get; private set; }
    public string Purpose { get; private set; }
    public string? OtherPurpose { get; private set; }
    public string? Remarks {  get; private set; }
    public string Status { get; private set; }
    public string? HRMDRequestStatus { get; private set; }
    public string? RMDRequestStatus { get; private set; }
    public string? FullName { get; set; }
    public virtual IList<RequestedRecord> RequestedRecords { get; set; }
    public virtual IList<RecordRequestDocument>? Files { get; set; }
    public virtual IList<RequestedRecordReceipt>? RequestedRecordReceipts { get; set; }    
}
