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

public class HomeBase : GridBase<EmployeeModel>
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
    public List<object> Series1Data;
    public string[] Categories;
    public TelerikChart FullfilledChartRef { get; set; }
    public TelerikChart RequstChartRef { get; set; }
     
    protected string TextSearchControlNumber = string.Empty;
    protected string TextSearchFullName = string.Empty;
    protected string TextSearchDateRequested = string.Empty;
    protected string TextSearchStatus = string.Empty;
    protected DateTime? SelectedDate { get; set; }

    protected List<string> StatusList = new List<string>
    {
        "Review",
        "Release",
        "Claimed"
    };
   
    public string DropDownListValue { get; set; }
    protected override void OnInitialized()
    {
       
        DropDownListValue = "All";

        EmployeeList = GenerateEmployeeRecords(5);
    }

    
    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        await HandleEndUserAccess();
        await GetRecordRequest();
        await GetOverviewTotal();
        await GetMonthlyRequestTotal();
        IsLoading = false;
    }
    protected void SetDateFilter(CompositeFilterDescriptor filterDescriptor)
    {
        filterDescriptor.FilterDescriptors.Clear();
        if (SelectedDate.HasValue)
        {
            var selectedDatePickerFrom = new FilterDescriptor(nameof(EmployeeModel.DateRequested), FilterOperator.IsGreaterThan, SelectedDate);
            var selectedDatePickerTo = new FilterDescriptor(nameof(EmployeeModel.DateRequested), FilterOperator.IsLessThan, SelectedDate.Value.AddDays(1));
             

            filterDescriptor.FilterDescriptors.Add(selectedDatePickerFrom);
            filterDescriptor.FilterDescriptors.Add(selectedDatePickerTo);
        }

        StateHasChanged();
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
             
            Series1Data = GetMonthlyRequestData.Select(d => (object)d.Count).ToList();
        }
    }

    private async Task GetRecordRequest()
    {
        var res = await RequestManagementService.Query(DataSourceReq);
 
        if(res.Data != null)
        {
            var getData = GenericHelper.GetListByDataSource<EmployeeModel>(res.Data);
            EmployeeRecords = getData;
        }
        
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
 
   
}
