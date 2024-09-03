using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Telerik.SvgIcons;
using DPWH.EDMS.IDP.Core.Constants;
using Telerik.Blazor.Components;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Pages.Home.HomeService;
using DPWH.EDMS.Client.Shared.Models;
using Telerik.Blazor;
using Telerik.DataSource;
using Telerik.Blazor.Components.Grid;

namespace DPWH.EDMS.Web.Client.Pages.Home;

public class HomeBase : GridBase<RecordRequestModel>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }
    [Inject] public required OverviewFilterService OverviewService { get; set; }
    protected Api.Contracts.DataSourceResult GetRequestManagementData { get; set; } = new Api.Contracts.DataSourceResult();
    protected List<EmployeeModel> EmployeeRecords { get; set; } = new List<EmployeeModel>();
    protected List<SimpleKeyValue> OverviewStatusList = new List<SimpleKeyValue>();
    protected List<SimpleKeyValue> OverviewTotalTopBar = new List<SimpleKeyValue>();
    protected List<SimpleKeyValue> OverviewTotalChart = new List<SimpleKeyValue>();
    protected List<SimpleKeyValue> SecondStatusList = new List<SimpleKeyValue>();
    protected List<EmployeeModel> EmployeeList = new List<EmployeeModel>();
    protected List<SimpleKeyValue> PieChart = new List<SimpleKeyValue>();
    protected List<GetMonthlyRequestModel> GetMonthlyRequestData { get; set; } = new List<GetMonthlyRequestModel>();
    protected List<GetTopRequestQueryModel> GetTopRequestList { get; set; } = new List<GetTopRequestQueryModel>();

    public List<object> Series1Data;
    public string[] Categories;
    public string[] HrmdCategories;

    public List<object> SeriesTopRequest;
    public string[] CategoriesTopRequest;
    public TelerikChart FullfilledChartRef { get; set; }
    public TelerikChart RequstChartRef { get; set; }
    public TelerikChart MonthlyAveTimeRef { get; set; }
    public TelerikChart Top10ChartRef { get; set; }
     
    protected DateTime? SelectedDate { get; set; }
    protected int? SearchControlNumber { get; set; }
    protected string? SearchFullName { get; set; }
    protected string? SearchPurpose { get; set; }
    protected string? SearchStatus { get; set; }
    protected int ValueAxisMax { get; set; } = 30;

    protected List<string> StatusList = new List<string>
    {
        "Submitted",
        "Reviewed",
        "Approved",
        "Released",
        "Claimed"
    };

    public string DropDownListValue { get; set; }
    protected override void OnInitialized()
    {

        DropDownListValue = "All";

        EmployeeList = GenerateEmployeeRecords(5);

        mediaQueryActions = new Dictionary<string, Action<bool>>
        {
            { nameof(XSmall), changed => XSmall = changed },
            { nameof(Small), changed => Small = changed },
            { nameof(Medium), changed => Medium = changed},
            { nameof(Large), changed => Large = changed }
        };
    }


    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        await HandleEndUserAccess();
        await GetRecordRequest();
        await GetOverviewTotal();
        await GetMonthlyRequestTotal();
        await GetTopRequest();

        IsLoading = false;
    }
    protected async void SetFilterGrid()
    {
        var filters = new List<Api.Contracts.Filter>();

        AddDateFilter(filters);
        AddTextSearchFilterIfNotNull(filters, nameof(RecordRequestModel.ControlNumber), SearchControlNumber?.ToString(), "eq");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordRequestModel.FullName), SearchFullName, "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordRequestModel.Status), SearchStatus, "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordRequestModel.Purpose), SearchPurpose, "contains");

        SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
        SearchFilterRequest.Filters = filters.Any() ? filters : null;

        await LoadData();
        StateHasChanged();
    }

    private void AddDateFilter(List<Api.Contracts.Filter> filters)
    {
        if (SelectedDate.HasValue)
        {
            AddTextSearchFilter(filters, nameof(RecordRequestModel.DateRequested), SelectedDate.Value.ToString(), "gte");
            AddTextSearchFilter(filters, nameof(RecordRequestModel.DateRequested), SelectedDate.Value.AddDays(1).ToString(), "lte");
        }
    }

    private void AddTextSearchFilterIfNotNull(List<Api.Contracts.Filter> filters, string fieldName, string? value, string operation)
    {
        if (!string.IsNullOrEmpty(value))
        {
            AddTextSearchFilter(filters, fieldName, value, operation);
        }
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

    private async Task GetMonthlyRequestTotal()
    {
        var res = await RequestManagementService.GetMonthlyRequestTotal();
        if (res.Success && res.Data != null)
        {
            GetMonthlyRequestData = res.Data.ToList();

            Categories = GetMonthlyRequestData.Select(d => d.Month).ToArray();
            HrmdCategories = new string[] { "Day 1", "Day 2", "Day 3", "Day 4", "Day 5", "Day 6", "Day 7" };

            Series1Data = GetMonthlyRequestData.Select(d => (object)d.Count).ToList();
             
            int maxValue = Series1Data.Cast<int>().Max();
            ValueAxisMax = maxValue < 28 ? 30 : maxValue;
        }
    }

    protected long GetHighestPercentageItem()
    {
        var total = GetTopRequestList.Select(x => (object)x.Total).ToList();

        if (!total.Any())
        {
            return 0;
        }

        long maxValue = total.Cast<long>().Max();
        return maxValue < 28 ? 30 : GetTopRequestList.OrderByDescending(item => item.Total).FirstOrDefault().Total;
    }
    private Dictionary<string, Action<bool>> mediaQueryActions;
    
    protected void HandleMediaQueryChange(string propertyName, bool changed)
    {
        if (mediaQueryActions.TryGetValue(propertyName, out var action))
        {
            action(changed);
            RequstChartRef.Refresh();
            MonthlyAveTimeRef.Refresh();
            Top10ChartRef.Refresh();
        }
    }
    private async Task GetRecordRequest()
    {
        ServiceCb = RequestManagementService.Query;
        await LoadData();
    }

    private async Task GetOverviewTotal()
    {
        var reviewTotal = await RequestManagementService.GetTotalOverviewStatus("review");
        var releaseTotal = await RequestManagementService.GetTotalOverviewStatus("release");
        var claimTotal = await RequestManagementService.GetTotalOverviewStatus("claimed");

        GetOverviewTotal(reviewTotal.Data.Total, releaseTotal.Data.Total, claimTotal.Data.Total);
    }

    private void GetOverviewTotal(int review, int release, int claimed)
    {
        //Display even if it's equal to zero
        OverviewTotalTopBar = new List<SimpleKeyValue> {
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

        //For chart : display only that have value
        OverviewTotalChart = new List<SimpleKeyValue> {
            new SimpleKeyValue
            {
                Id = "Review",
                Name = review != 0 ? $"{review}" : null,
            },
            new SimpleKeyValue
            {
                Id = "Release",
                Name = release != 0 ? $"{release}" : null,
            },
            new SimpleKeyValue
            {
                Id = "Claimed",
                Name = claimed != 0 ? $"{claimed}" : null,
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

    protected async Task GetTopRequest()
    {
        var res = await RequestManagementService.GetTopRequestRecords();
        if(res.Success && res.Data != null)
        {
            GetTopRequestList = res.Data.ToList();

            CategoriesTopRequest = GetTopRequestList.Select(d => d.RecordName).ToArray();

            SeriesTopRequest = GetTopRequestList.Select(d => (object)d.Total).ToList();
        }
    }
    protected void GoToSelectedItem(GridRowClickEventArgs args)
    {
        IsLoading = true;

        var selectedItem = args.Item as RecordRequestModel;

        if (selectedItem != null)
        {
            NavManager.NavigateTo("request-management/view-request-form/" + selectedItem.Id.ToString());
        }

        IsLoading = false;
    }
}
