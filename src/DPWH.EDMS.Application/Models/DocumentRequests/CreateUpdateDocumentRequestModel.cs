using DPWH.EDMS.Domain;

namespace DPWH.EDMS.Application;

public class CreateUpdateDocumentRequestModel
{
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

public static class CreateUpdateDocumentRequestMapper
{
    public static DocumentRequest MapToEntity(CreateUpdateDocumentRequestModel model, Guid employeeRecordsId, Guid documentRecordsId)
    {
        return new DocumentRequest()
        {
            Id = Guid.NewGuid(),
            EmployeeNumber = model.EmployeeNumber,
            ControlNumber = model.ControlNumber,
            EmployeeRecordsId = employeeRecordsId,
            ClaimedBy = model.ClaimedBy,
            AuthorizedRepresentative = model.AuthorizedRepresentative,
            ValidId = model.ValidId,
            SupportingDocument = model.SupportingDocument,
            DocumentRecordsId = documentRecordsId,
            DateRequested = model.DateRequested,
            RequestedRecord = model.RequestedRecord,
            Purpose = model.Purpose,
            Status = model.Status,
        };
    }
}
