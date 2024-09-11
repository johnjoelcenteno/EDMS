
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.Reports;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.Reports.Users.Model;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Web.Client.Shared.Services.Export;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.FontIcons;

namespace DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.Reports.Users;

public class UserBase : GridBase<UserReportsModel>
{
    #region
    [Inject] public required ILookupsService LookUpService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IReportsService ReportsService {  get; set; }
    [Inject] public required IExcelExportService ExcelExportService { get; set; }
    #endregion

    protected List<string> UserTypeList {  get; set; } = new List<string>();
    protected List<string> SelectedTypes { get; set; } = new List<string>();
  
    protected readonly string TwoFormFieldsResponsiveClass = "col-12 col-md-6 col-lg-3";

    protected string SelectedRegionalOffice { get; set; }
    protected string SelectedDEO { get; set; }
    protected List<GetRequestingOfficeResult> RequestingOffice = new List<GetRequestingOfficeResult>();
    protected List<GetRequestingOfficeResultItem> DEOList { get; set; } = new List<GetRequestingOfficeResultItem>();
    protected TelerikDropDownList<GetRequestingOfficeResult, string> ImplementDropRef = new();
    protected bool Visible { get; set; } = false;
    protected override async Task OnInitializedAsync()
    {
        base.OnInitialized();
        await GetRequestingOffices();
        BreadcrumbItems = new List<BreadcrumbModel>
            {
                new() { Icon = FontIcon.Home.ToString(), Url = "/" },
                new() { Text = "Reports", Icon = FontIcon.Menu.ToString() },
                new() { Text = "User", Icon = FontIcon.TableUnmerge.ToString() }
            };

        UserTypeList = ApplicationRoles.AssignableRoles.Select(ar => ar.Value).ToList();
    }

    protected async Task GetRequestingOffices()
    {
        var res = await ExceptionHandlerService.HandleApiException<GetRequestingOfficeResultIEnumerableBaseApiResponse>(async () => await LookUpService.GetRequestingOfficeList(), null);
        if (res != null && res.Success)
        {
            RequestingOffice = res.Data.ToList();
        }
    }

    protected async void OnChangeRegionOffice()
    {
        if (string.IsNullOrEmpty(SelectedRegionalOffice))
        {
            SelectedDEO = "";
        }
        if (SelectedRegionalOffice != null)
        {

            DEOList = RequestingOffice
                    .Where(x => x.RegionName == SelectedRegionalOffice)
                    .SelectMany(x => x.ImplementingOffices)
                    .ToList();
        }
    }

    protected async Task ShowReport(List<string> selectedTypes)
    {
        await ExceptionHandlerService.HandleApiException(async () =>
        {
            IsLoading = true;

            ServiceCb = ReportsService.QueryUser;
            var filters = GetFilters(selectedTypes, SelectedRegionalOffice);

            SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
            SearchFilterRequest.Filters = filters;
            await LoadData();
            if (string.IsNullOrEmpty(GridData?.FirstOrDefault()?.UserAccess))
            {
                ToastService.ShowError("No Data Found");
            }
            IsLoading = false;
        });
           
       
    }
    private List<Filter> GetFilters(List<string> selectedTypes, string regionalOffice)
    {
        var filters = new List<Filter>();

        if (selectedTypes != null)
        {
            var groupFilters = selectedTypes
                .Select(item => CreateTextSearchFilter(nameof(UserReportsModel.UserAccess), item, "contains"))
                .ToList();

            if (groupFilters.Count > 1)
            {
                filters.Add(new Filter
                {
                    Logic = DataSourceHelper.OR_LOGIC,
                    Filters = groupFilters
                });
            }
            else if (groupFilters.Count == 1)
            {
                filters.Add(groupFilters.Single());
            }
        }

        if (!string.IsNullOrEmpty(regionalOffice))
        {
            filters.Add(CreateTextSearchFilter(nameof(UserReportsModel.SubOffice), regionalOffice, "eq"));
        }

        //if (!string.IsNullOrEmpty(deo))
        //{
        //    filters.Add(CreateTextSearchFilter(nameof(UserReportsModel.Office), deo, "eq"));
        //}

        return filters;
    }

    private Filter CreateTextSearchFilter(string fieldName, string value, string comparison)
    {
        return new Filter
        {
            Field = fieldName,
            Value = value,
            Operator = comparison
        };
    }

    protected async Task ConfirmToExcel()
    {
        IsLoading = true;
        var fileName = $"User Report Type as of {DateTime.Now.ToString("MMM dd, yyyy")}.xlsx";
        var items = await GetAllItems();

        await ExceptionHandlerService.HandleApiException(async () => await ExcelExportService.ExportList(items, fileName), null);
        //await _ExcelExportService.ExportList(items, fileName);

        IsLoading = false;
    }
    protected async Task<List<UserReportsModel>> GetAllItems()
    {

        var items = await GenericHelper.GetListByQuery<UserReportsModel>(
            new DataSourceRequest() { Skip = 0, Filter = DataSourceReq.Filter },
            ReportsService.QueryUser,
            (err) => ToastService.ShowError(err)
        );

        return items;
    }
}
