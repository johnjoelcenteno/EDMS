namespace DPWH.EDMS.Application.Features.Users.Commands.CreateUserWithRole;

public record CreateUserWithRoleResult
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleInitial { get; set; }
    public string? LastName { get; set; }
    public string? MobileNumber { get; set; }
    public required string Role { get; set; }
    public required string UserAccess { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? RegionalOfficeRegion { get; set; }
    public string? RegionalOfficeProvince { get; set; }
    public string? DistrictEngineeringOffice { get; set; }
    public string? DesignationTitle { get; set; }
    public required string CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}