namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.CreateRecordRequest;

/// <summary>
/// ValidId - the Id of the uploaded Valid ID document
/// SupportingDocument - the id of uploaded Supporting document
/// </summary>
/// <param name="EmployeeNumber"></param>
/// <param name="ControlNumber"></param>
/// <param name="IsActiveEmployee"></param>
/// <param name="Claimant"></param>
/// <param name="DateRequested"></param>
/// <param name="AuthorizedRepresentative"></param>
/// <param name="ValidId"></param>
/// <param name="SupportingDocument"></param>
/// <param name="RequestedRecords"></param>
/// <param name="Purpose"></param>
public record CreateRecordRequest(string EmployeeNumber, string ControlNumber, bool IsActiveEmployee,
    string Claimant, DateTimeOffset DateRequested, string? AuthorizedRepresentative, Guid? ValidId, Guid? SupportingDocument,
    Guid[] RequestedRecords, string Purpose);

