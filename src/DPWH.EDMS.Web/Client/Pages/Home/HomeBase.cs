using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Components.ReusableGrid;

namespace DPWH.EDMS.Web.Client.Pages.Home;

public class HomeBase : GridBase<EmployeeModel>
{
    protected List<SimpleKeyValue> OverviewStatusList = new List<SimpleKeyValue>();
    protected List<EmployeeModel> EmployeeList = new List<EmployeeModel>();

    protected override void OnInitialized()
    {
        OverviewStatusList = new List<SimpleKeyValue> {
            new SimpleKeyValue()
            {
                Id = "FOR REVIEW",
                Name = "02",
            },
            new SimpleKeyValue()
            {
                Id = "FOR REVIEW",
                Name = "02",
            },
            new SimpleKeyValue()
            {
                Id = "APPROVED",
                Name = "02",
            },
             new SimpleKeyValue()
            {
                Id = "For Release",
                Name = "02",
            },
            new SimpleKeyValue()
            {
                Id = "Received",
                Name = "05",
            },
        };

        EmployeeList = GenerateEmployeeRecords(5);
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
                FullName = $"FirstName{i} M. LastName{i}",
                MiddleInitial = "M",
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
