namespace DPWH.EDMS.Application.Features.RecordRequests.Queries;
public record RecordRequestModel
{
    public Guid Id { get; set; }
    public int ControlNumber { get; set; }
    public string EmployeeNumber { get; set; }
    public string ClaimantType { get; set; }
    public DateTimeOffset DateRequested { get; set; }
    public AuthorizedRepresentativeModel? AuthorizedRepresentative { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
    public string? FullName { get; set; }
    public List<RecordRequestDocumentModel> Files { get; set; } = new();
    public List<RequestedRecordModel> RequestedRecords { get; set; } = new();
}

public record AuthorizedRepresentativeModel
{
    public Guid? SupportingFileValidId { get; set; }
    public Guid? SupportingFileAuthorizationDocumentId { get; set; }
    public string? RepresentativeName { get; set; }
    public Guid? ValidId { get; set; }
    public string? ValidIdName { get; set; }
    public string? ValidIdUri { get; set; }
    public Guid? AuthorizationDocumentId { get; set; }
    public string? AuthorizationDocumentName { get; set; }
    public string? AuthorizationDocumentUri { get; set; }
}

public record RequestedRecordModel(Guid Id, Guid RecordTypeId, string RecordType, string Office, string Status, bool IsAvailable, string? Uri);

public record RequestedRecordSummaryModel(Guid RecordTypeId, string RecordType);
