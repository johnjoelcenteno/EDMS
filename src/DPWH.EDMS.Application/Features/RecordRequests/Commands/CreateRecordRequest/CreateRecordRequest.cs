namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.CreateRecordRequest;

/// <summary>
/// SupportingFileValidId - the Id of the uploaded Valid ID document from [RecordRequestDocuments]
/// SupportingFileAuthorizationDocumentId - the id of uploaded Authorization document from [RecordRequestDocuments]
/// </summary>
/// <param name="EmployeeNumber"></param>
/// <param name="Claimant"></param>
/// <param name="DateRequested"></param>
/// <param name="AuthorizedRepresentative"></param>
/// <param name="SupportingFileValidId"></param>
/// <param name="SupportingFileAuthorizationDocumentId"></param>
/// <param name="RequestedRecords"></param>
/// <param name="Purpose"></param>
public record CreateRecordRequest(string EmployeeNumber, string Claimant, DateTimeOffset DateRequested, string? AuthorizedRepresentative,
    Guid? SupportingFileValidId, Guid? SupportingFileAuthorizationDocumentId, Guid[] RequestedRecords, string Purpose);

