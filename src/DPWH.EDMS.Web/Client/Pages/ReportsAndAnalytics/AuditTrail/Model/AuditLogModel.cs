namespace DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.Model;

public class AuditLogModel
{
    public string EntityId { get; set; }
    public string Entity { get; set; }
    public string PropertyId { get; set; }
    public string PropertyName { get; set; }
    public string TargetUser { get; set; }
    public string BuildingId { get; set; }
    public string Action { get; set; }
    public string CreatedBy { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string EmployeeNumber { get; set; }
    public DateTime Created { get; set; }
    public List<ChangeModel> Changes { get; set; }
}

public class ChangeModel
{
    public string Field { get; set; }
    public string From { get; set; }
    public string To { get; set; }
}