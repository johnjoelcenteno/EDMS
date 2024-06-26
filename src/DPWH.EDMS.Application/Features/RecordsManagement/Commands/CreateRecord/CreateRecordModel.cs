namespace DPWH.EDMS.Application.Features.RecordsManagement.Commands.CreateRecord;

/// <summary>
/// Parameters for creating a Record
/// </summary>
/// <param name="EmployeeId"></param>
/// <param name="RecordTypeId"></param>
/// <param name="RecordName"></param>
/// <param name="RecordUri"></param>

public record CreateRecordModel(string EmployeeId, Guid RecordTypeId, string RecordName, string RecordUri);

