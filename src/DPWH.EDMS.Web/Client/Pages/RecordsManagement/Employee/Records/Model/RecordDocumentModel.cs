namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.Employee.Records.Model;
public class RecordDocumentModel
{
    public Guid Id { get; set; }
    public string EmployeeId { get; set; }
    public Guid RecordTypeId { get; set; }
    public string RecordName { get; set; }
    public string RecordUri { get; set; }
    public string DocVersion { get; set; }
}