namespace DPWH.EDMS.Application.Features.Reports.Queries;

public class UserReportModel
{
    public string Id { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
    public string? UserAccess { get; set; }
    public string? Position { get; set; }
    public string? Email { get; set; }
    public string? SubOffice { get; set; }
    public string? Office { get; set; }
    public DateTimeOffset TimeIn { get; set; }
    public DateTimeOffset TimeOut { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Location { get; set; }
    public string? Gender { get; set; }



}