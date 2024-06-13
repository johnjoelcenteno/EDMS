namespace DPWH.EDMS.Application.Features.RecordRequests.Queries;
public record RecordRequestModel
{
    public Guid Id { get; set; }
    public string ControlNumber { get; set; }
    public string EmployeeNumber { get; set; }
    public bool IsActiveEmployee { get; set; }
    public string ClaimantType { get; set; }
    public DateTimeOffset DateRequested { get; set; }
    public AuthorizedRepresentativeModel? AuthorizedRepresentative { get; set; }
    public string RequestedRecord { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
    public List<RecordRequestDocumentModel> Files { get; set; } = new();
    public List<RequestedRecordModel> RequestedRecords { get; set; } = new();
}

public record AuthorizedRepresentativeModel
{    
    public string? RepresentativeName { get; set; }
    public string? ValidId { get; set; }
    public string? ValidIdUri { get; set; }
    public string? SupportingDocument { get; set; }
    public string? SupportingDocumentUri { get; set; }
}

public record RequestedRecordModel(Guid RecordTypeId, string RecordType);
