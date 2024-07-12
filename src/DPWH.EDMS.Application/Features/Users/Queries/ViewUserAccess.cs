namespace DPWH.EDMS.Application.Features.Users.Queries;

/// <summary>
/// Use to quickly search user access details including their role
/// </summary>
public record ViewUserAccess(
    string Id,
    string UserName,
    string Email,
    string FirstName,
    string? MiddleInitial,
    string LastName,
    string? EmployeeId,
    string? MobileNumber,
    string? UserRole,
    string UserAccess,
    string? Department,
    string? Position,
    string? RegionalOfficeRegion,
    string? RegionalOfficeProvince,
    string? DistrictEngineeringOffice,
    string? DesignationTitle,
    string? Office,
    DateTimeOffset? Created,
    string? CreatedBy);