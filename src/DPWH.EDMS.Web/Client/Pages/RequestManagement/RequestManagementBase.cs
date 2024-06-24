using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Web.Client.Pages.CurrentUser.PendingRequests.RequestForm;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement;

public class RequestManagementBase : GridBase<EmployeeModel>
{
    protected List<EmployeeModel> EmployeeList = new List<EmployeeModel>();
    public FilterOperator filterOperator { get; set; } = FilterOperator.StartsWith;

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Request Management",
            Url = "/Request-management"
        });

        EmployeeList = GenerateEmployeeRecords(5);
    }

    public List<FilterOperator> filterOperators { get; set; } = new List<FilterOperator>()
    {
        FilterOperator.IsEqualTo,
        FilterOperator.IsNotEqualTo,
        FilterOperator.StartsWith,
        FilterOperator.Contains,
        FilterOperator.DoesNotContain
    };
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
                LastName = $"Office{i}",
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
    protected void GoToReviewRequestForm(GridRowClickEventArgs args)
    {
        var selectedControlNumber = args.Item as EmployeeModel;

        if (selectedControlNumber != null)
        {
            NavManager.NavigateTo("/request-management/review-request/" + selectedControlNumber.ControlNumber);
        }
    }
}
