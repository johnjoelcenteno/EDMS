namespace DPWH.EDMS.Domain;

public class EmployeeRecords
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Office { get; set; }
    public string Email { get; set; }
    public string MobileNumber { get; set; }
    public string EmployeeNumber { get; set; }
    public string RegionCentralOffice { get; set; }
    public string DistrictBureauService { get; set; }
    public string Position { get; set; }
    public string Designation { get; set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset Modified { get; set; }
    public string ModifiedBy { get; set; }
}
