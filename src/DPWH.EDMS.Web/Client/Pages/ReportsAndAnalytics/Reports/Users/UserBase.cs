using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.Reports.Users.Model;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.FontIcons;

namespace DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.Reports.Users;

public class UserBase : GridBase<UserReportsModel>
{
    #region
    [Inject] private ILookupsService _LookUpService { get; set; } = default!;
    [Inject] private IToastService _ToastService { get; set; } = default!;
    [Inject] protected IExceptionHandlerService ExceptionHandlerService { get; set; } = default!;
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
        var res = await ExceptionHandlerService.HandleApiException<GetRequestingOfficeResultIEnumerableBaseApiResponse>(async () => await _LookUpService.GetRequestingOfficeList(), null);
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
        //try
        //{
        //    IsLoading = true;

        //    ServiceCb = _ReportService.UsersQuery;
        //    var filters = GetFilters(selectedTypes, SelectedRegionalOffice, SelectedDEO);

        //    SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
        //    SearchFilterRequest.Filters = filters;
        //    await LoadData();
        //    if (string.IsNullOrEmpty(GridData?.FirstOrDefault()?.UserAccess))
        //    {
        //        _ToastService.ShowError("No Data Found");
        //    }
        //}
        //catch (ApiException<ProblemDetails> apiExtension)
        //{
        //    HandleApiException(apiExtension);
        //}
        //finally
        //{
        //    IsLoading = false;
        //}
    }
}
