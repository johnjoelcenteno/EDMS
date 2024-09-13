namespace DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.Reports.UserManagement.Model
{
    public class UserManagementReportsModel
    {
        public string? EmployeeId { get; set; }
        public string? UserAccess { get; set; }
        public string? Position { get; set; } // Added field
        public string? Email { get; set; } // Added field
        public string? SubOffice { get; set; } // Added field
        public string? Office { get; set; } // Added field
        public DateTimeOffset? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleInitial { get; set; }
        public string? LastName { get; set; }
        public string EmployeeFullName => $"{FirstName} {CheckMiddleInitial()} {LastName}";
        public string CheckMiddleInitial() => !string.IsNullOrEmpty(MiddleInitial) ? $"{MiddleInitial}." : "";
    }
}
