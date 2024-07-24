namespace DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.Reports.Users.Model;

public class UserReportsModel
{
    public string? UserAccess { get; set; }
    public string? Position { get; set; } // Added field
    public string? Email { get; set; } // Added field
    public string? SubOffice { get; set; } // Added field
    public string? Office { get; set; } // Added field
    public DateTimeOffset? CreatedDate { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}