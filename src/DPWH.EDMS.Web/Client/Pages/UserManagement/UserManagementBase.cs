using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Telerik.FontIcons;

namespace DPWH.EDMS.Web.Client.Pages.UserManagement;

public class UserManagementBase : GridBase<EmployeeModel>
{
    protected double LicenseLimit = 15;
    protected double LicenseAccumulated = 1;
    protected double TotalUsers = 633;

    protected List<EmployeeModel> EmployeeList = new List<EmployeeModel>();

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = FontIcon.Rows,
            Text = "User Management",
            Url = "/user-management"
        });

        EmployeeList = GenerateEmployeeRecords(5);
    }

    protected double GetLicenseAccumulatedPercentage()
    {
        return Math.Round((LicenseAccumulated / LicenseLimit) * 100, 2);
    }

    private List<EmployeeModel> GenerateEmployeeRecords(int count)
    {
        var employees = new List<EmployeeModel>();
        var random = new Random();

        for (int i = 1; i <= count; i++)
        {
            var employee = new EmployeeModel
            {
                ControlNumber = $"CN{i:D4}",
                DateRequested = DateTime.Now.AddDays(-random.Next(0, 365)),
                LastName = $"LastName{i}",
                FirstName = $"FirstName{i}",
                FullName = $"FirstName{i} M. LastName{i}",
                MiddleInitial = "M",
                Email= $"example{i}@sample.com",
                RecordRequested = $"RecordType{random.Next(1, 5)}",
                Purpose = $"Purpose{random.Next(1, 5)}",
                Status = random.Next(0, 2) == 0 ? "Pending" : "Completed",
                UserAccess = "EndUser"
            };

            employees.Add(employee);
        }

        return employees;
    }
}
