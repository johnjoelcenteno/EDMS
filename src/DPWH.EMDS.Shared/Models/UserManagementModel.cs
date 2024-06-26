using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Client.Shared.Models;

public record UserManagementModel
{
    [Required(ErrorMessage = "EmployeeID is required")]
    public string EmployeeId { get; set; }
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }
    public string MiddleName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; }
    //[Required(ErrorMessage = "Department is required")]
    public string Department { get; set; }
    //[Required(ErrorMessage = "Position is required")]
    public string Position { get; set; }
    //[Required(ErrorMessage = "Regional Office is required")]
    public string RegionalOffice { get; set; }
    //[Required(ErrorMessage = "District Engineering Office is required")]
    public string DistrictEngineeringOffice { get; set; }
    //[Required(ErrorMessage = "Designation Office is required")]
    public string DesignationTitle { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? Created { get; set; }
    public string FullName { get; set; }
}

