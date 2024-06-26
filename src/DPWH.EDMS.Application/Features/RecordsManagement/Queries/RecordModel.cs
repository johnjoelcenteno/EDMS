namespace DPWH.EDMS.Application.Features.RecordsManagement.Queries;
public record RecordModel
{
    public Guid Id { get; set; }
    public string EmployeeId { get; set; }
    public Guid RecordTypeId { get; set; }
    public string RecordName { get; set; }
    public string RecordUri { get; set; }
}

