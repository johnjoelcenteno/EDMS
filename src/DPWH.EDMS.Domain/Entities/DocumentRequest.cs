using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain;

public class DocumentRequest : EntityBase
{
    private DocumentRequest() { }

    public static DocumentRequest Create(
       string employeeNumber,
       string controlNumber,
       Guid employeeRecordsId,
       string claimedBy,
       string authorizedRepresentative,
       string validId,
       string supportingDocument,
       Guid documentRecordsId,
       DateTimeOffset dateRequested,
       string requestedRecord,
       string purpose,
       string status,
       string createdBy
    )
    {
        var entity = new DocumentRequest
        {
            EmployeeNumber = employeeNumber,
            ControlNumber = controlNumber,
            EmployeeRecordsId = employeeRecordsId,
            ClaimedBy = claimedBy,
            AuthorizedRepresentative = authorizedRepresentative,
            ValidId = validId,
            SupportingDocument = supportingDocument,
            DocumentRecordsId = documentRecordsId,
            DateRequested = dateRequested,
            RequestedRecord = requestedRecord,
            Purpose = purpose,
            Status = status,
        };

        entity.SetCreated(createdBy);
        return entity;
    }

    private string EmployeeNumber { get; set; }
    private string ControlNumber { get; set; }
    private Guid EmployeeRecordsId { get; set; }
    private string ClaimedBy { get; set; }
    private string AuthorizedRepresentative { get; set; }
    private string ValidId { get; set; }
    private string SupportingDocument { get; set; }
    private Guid DocumentRecordsId { get; set; }
    private DateTimeOffset DateRequested { get; set; }
    private string RequestedRecord { get; set; }
    private string Purpose { get; set; }
    private string Status { get; set; }
}
