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
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Pages.Home.HomeService;
using DPWH.EDMS.Client.Shared.Models;
using Telerik.Blazor;

namespace DPWH.EDMS.Web.Client.Pages.Home;

public class HomeBase : GridBase<EmployeeModel>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }
    [Inject] public required OverviewFilterService OverviewService { get; set; }

    protected List<SimpleKeyValue> OverviewStatusList = new List<SimpleKeyValue>();
    protected List<SimpleKeyValue> OverviewTotal = new List<SimpleKeyValue>();
    protected List<SimpleKeyValue> SecondStatusList = new List<SimpleKeyValue>();
    protected List<EmployeeModel> EmployeeList = new List<EmployeeModel>();
    protected List<SimpleKeyValue> PieChart = new List<SimpleKeyValue>();
    public List<object> Series1Data;
    public string[] Categories;
    public TelerikChart FullfilledChartRef { get; set; }
    public TelerikChart RequstChartRef { get; set; }
     
    protected string TextSearchControlNumber = string.Empty;
    protected string TextSearchFullName = string.Empty;
    protected string TextSearchDateRequested = string.Empty;
    protected string TextSearchStatus = string.Empty;
    public FilterOperator filterOperator { get; set; } = FilterOperator.Contains;
    protected DateTime? SelectedDate { get; set; }

    protected List<string> StatusList = new List<string>
    {
        "Review",
        "Release",
        "Claimed"
    };

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
       
        Categories = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"  };
        Series1Data = new List<object>() { 250, 440, 580, 660, 553, 311, 600, 580, 600, 770, 910, 820 };

        DropDownListValue = "All";

        EmployeeList = GenerateEmployeeRecords(5);
    }

    
    protected async override Task OnInitializedAsync()
    {
        await HandleEndUserAccess();
        await GetRecordRequest();
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
    private async Task GetRecordRequest()
    {
        ServiceCb = RequestManagementService.Query;

        await LoadData();

        await GetOverviewTotal();
    }
    private async Task GetOverviewTotal()
    {
        var filter = OverviewService.GetFilterOverviewBanner();

        var res = await RequestManagementService.Query(new Api.Contracts.DataSourceRequest() { Filter = filter });
         
        if (res != null)
        {
            var getList = GenericHelper.GetListByDataSource<EmployeeModel>(res.Data);

            var reviewTotal = getList.Where(x => x.Status == "Review").Count();
            var releaseTotal = getList.Where(x => x.Status == "Release").Count();
            var claimTotal = getList.Where(x => x.Status == "Claimed").Count();

            GetOverviewTotal(reviewTotal, releaseTotal, claimTotal);
        }
    }
   
    private void GetOverviewTotal(int review, int release, int claimed)
    {
        OverviewTotal = new List<SimpleKeyValue> {
            new SimpleKeyValue()
            {
                Id = "Review",
                Name = $"{review}",
            },
            new SimpleKeyValue()
            {
                Id = "Release",
                Name = $"{release}",
            },
            new SimpleKeyValue()
            {
                Id = "Claimed",
                Name = $"{claimed}",
            }
        };
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

    protected async Task SearchFilter()
    {
        var filters = new List<Api.Contracts.Filter>(); 
        if (!string.IsNullOrEmpty(TextSearchControlNumber))
        {
            AddTextSearchFilter(filters, nameof(EmployeeModel.ControlNumber), TextSearchControlNumber, "eq");
        }

        if (!string.IsNullOrEmpty(TextSearchFullName))
        {
            AddTextSearchFilter(filters, nameof(EmployeeModel.FullName), TextSearchFullName.ToLower());
        }

        if (SelectedDate.HasValue)
        {
            AddTextSearchFilter(filters, "DateRequested", SelectedDate.Value.ToString("yyyy-MM-dd"), "gte");
            AddTextSearchFilter(filters, "DateRequested", SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd"), "lte");
        }

        if (!string.IsNullOrEmpty(TextSearchStatus))
        {
            AddTextSearchFilter(filters, nameof(EmployeeModel.Status), TextSearchStatus.ToLower());
        }

        SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;

        SearchFilterRequest.Filters = filters;

        await LoadData();
    }
    protected string LastOnChangeValue { get; set; } 
    protected async Task OnStatusFilterChangeHandler(object input)
    {
        string CurrentValue = input as string;

        if (CurrentValue != LastOnChangeValue && string.IsNullOrEmpty(CurrentValue))
        {
            TextSearchStatus = string.Empty;
            LastOnChangeValue = string.Empty;
            await SearchFilter();
        }
        else if (!CurrentValue.Equals(LastOnChangeValue))
        {
            LastOnChangeValue = CurrentValue;
            await SearchFilter();
        }
    }
}
