using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Client.Shared.MockModels;

public class DocumentRequestModel
{
    public string ControlNumber { get; set; }
    public List<string> RecordsRequested { get; set; }
    public DateTime DateRequested { get; set; }
    public string? Purpose { get; set; }
    public string? Status { get; set; }
    public string EmployeeNo { get; set; }
    public bool IsActive { get; set; }
    public DocumentClaimant DocumentClaimant { get; set; }
    public string? AuthorizedRepresentative { get; set; }
    public string ValidIdType { get; set; }
    public string SupportingDocumentType { get; set; }
    public string? Remarks {  get; set; }
    public ValidId? ValidId {  get; set; }
    public bool IsValidIdAccepted { get; set; }
    public bool IsSupportedDocumentAccepted { get; set; }
}

public enum DocumentClaimant
{
    Requestor,
    AuthorizedRepresentative
}

public class ValidId
{
    public string Type { get; set; }
    public FileParameter? File { get; set; }
}



