using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Shared.Enums;

namespace DPWH.EDMS.Domain.Entities;

public class RecordRequest : EntityBase
{
    private RecordRequest() { }

    public static RecordRequest Create(       
       string controlNumber,
       string employeeNumber,
       ClaimantTypes claimantType,
       bool isActiveEmployee,
       DateTimeOffset dateRequested,
       AuthorizedRepresentative? representative,       
       string purpose,       
       string createdBy
    )
    {
        var entity = new RecordRequest
        {
            ControlNumber = controlNumber,
            EmployeeNumber = employeeNumber,
            ClaimantType = claimantType.ToString(),
            IsActiveEmployee = isActiveEmployee,
            DateRequested = dateRequested,
            AuthorizedRepresentative = representative,
            Purpose = purpose,
            Status = RecordRequestStates.Submitted.ToString(),
            RequestedRecords = []
        };
        entity.SetCreated(createdBy);
        return entity;
    }    
    public string ControlNumber { get; private set; }
    public string EmployeeNumber { get; private set; }
    public bool IsActiveEmployee { get; private set; }
    public string ClaimantType { get; private set; }
    public DateTimeOffset DateRequested { get; private set; }
    public AuthorizedRepresentative? AuthorizedRepresentative { get; private set; }    
    public string Purpose { get; private set; }
    public string Status { get; private set; }
    public virtual IList<RequestedRecord> RequestedRecords { get; set; }
    public virtual IList<RecordRequestDocument>? Files { get; set; }
}
