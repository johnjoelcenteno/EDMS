namespace DPWH.EDMS.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdResult
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? EmployeeId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleInitial { get; set; }
    public string? LastName { get; set; }
    public string? MobileNumber { get; set; }
    public string? Role { get; set; }
    public string? UserAccess { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? RegionalOfficeRegion { get; set; }
    public string? RegionalOfficeProvince { get; set; }
    public string? DistrictEngineeringOffice { get; set; }
    public string? DesignationTitle { get; set; }
    public string? Office {  get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}