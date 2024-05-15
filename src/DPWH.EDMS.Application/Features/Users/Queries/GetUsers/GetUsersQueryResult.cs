namespace DPWH.EDMS.Application.Features.Users.Queries.GetUsers;

public record GetUsersQueryResult
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleInitial { get; set; }
    public required string LastName { get; set; }
    public string? EmployeeId { get; set; }
    public string? MobileNumber { get; set; }
    public string? Role { get; set; }
    public required string UserAccess { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? RegionalOfficeRegion { get; set; }
    public string? RegionalOfficeProvince { get; set; }
    public string? DistrictEngineeringOffice { get; set; }
    public string? DesignationTitle { get; set; }
    public DateTimeOffset? Created { get; set; }
    public string? CreatedBy { get; set; }
}