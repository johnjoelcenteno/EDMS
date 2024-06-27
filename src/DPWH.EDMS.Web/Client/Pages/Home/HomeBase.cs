using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Telerik.SvgIcons;
using DPWH.EDMS.IDP.Core.Constants;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace DPWH.EDMS.Web.Client.Pages.Home;

public class HomeBase : GridBase<EmployeeModel>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }    

    protected List<SimpleKeyValue> OverviewStatusList = new List<SimpleKeyValue>();
    protected List<SimpleKeyValue> SecondStatusList = new List<SimpleKeyValue>();
    protected List<EmployeeModel> EmployeeList = new List<EmployeeModel>();
    protected List<SimpleKeyValue> PieChart = new List<SimpleKeyValue>();
    public List<object> Series1Data;
    public string[] Categories;
    public TelerikChart FullfilledChartRef { get; set; }
    public TelerikChart RequstChartRef { get; set; }

    public FilterOperator filterOperator { get; set; } = FilterOperator.StartsWith;

    public List<FilterOperator> filterOperators { get; set; } = new List<FilterOperator>()
    {
        FilterOperator.IsEqualTo,
        FilterOperator.IsNotEqualTo,
        FilterOperator.StartsWith,
        FilterOperator.Contains,
        FilterOperator.DoesNotContain
    };
    public string DropDownListValue { get; set; }
    protected override void OnInitialized()
    {
        OverviewStatusList = new List<SimpleKeyValue> {
            new SimpleKeyValue()
            {
                Id = "Review",
                Name = "200",
            },
            new SimpleKeyValue()
            {
                Id = "Release",
                Name = "130",
            },
            new SimpleKeyValue()
            {
                Id = "Claimed",
                Name = "320",
            }
        };
        Categories = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"  };
        Series1Data = new List<object>() { 5000, 7000, 12000, 10000, 19000, 24000, 18000, 30000, 34000, 28000, 36000, 41000 };

        DropDownListValue = "All";

        EmployeeList = GenerateEmployeeRecords(5);
    }

    protected async override Task OnInitializedAsync()
    {
        await HandleEndUserAccess();
    }

    private async Task HandleEndUserAccess()
    {
        var authState = await AuthenticationStateAsync!;
        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            if (user.IsInRole(ApplicationRoles.EndUser))
            {
                NavigationManager.NavigateTo("/my-requests");
            }
        }
    }

    private List<EmployeeModel> GenerateEmployeeRecords(int count)
    {
        var employees = new List<EmployeeModel>();
        var random = new Random();

        for (int i = 1; i <= count; i++)
        {
            var employee = new EmployeeModel
            {
                ControlNumber = $"2024{i:D6}",
                DateRequested = DateTime.Now.AddDays(-random.Next(0, 365)),
                LastName = $"LastName{i}",
                FullName = $"LastName{i} FirstName{i} M.",
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
