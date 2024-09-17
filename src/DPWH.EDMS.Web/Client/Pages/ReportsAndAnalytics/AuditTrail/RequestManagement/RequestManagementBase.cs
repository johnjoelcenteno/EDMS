using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.AuditLog;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.Configurations;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.AuditTrailGridBase;
using DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.Model;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Web.Client.Shared.Services.Export.ExportAuditTrail;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Breadcrumb;

namespace DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.RequestManagement;

public class RequestManagementBase : AuditTrailsGridBase<AuditLogModel>
{
    [Inject] public required IAuditLogService AuditLog { get; set; }
    [Inject] public required IAuditTrailExportService AuditLogExportService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    [Inject] public required NavigationManager NavManager { get; set; }
    protected DateTime dateFrom { get; set; } = DateTime.Now.AddDays(-1);
    protected DateTime dateTo { get; set; } = DateTime.Now;
    protected string SelectedType { get; set; } = "User";
    protected bool XSmall { get; set; }
    protected List<int?> PageSizes { get; set; } = new List<int?> { 5, 10, 15 };
    protected List<string> ReportType { get; set; } = new();
    protected UserActivityModel UserActivityData = new UserActivityModel();
    [Inject] public required ConfigManager ConfigManager { get; set; }
    public List<BreadcrumbModel> BreadcrumbItems { get; set; } = new()
    {
         new() { Icon = "home", Url = "/"},
    };
    protected string SelectedReportType { get; set; }
    protected List<GetLookupResult> Purposes { get; set; } = new List<GetLookupResult>();
    protected string SelectedPurpose = "";
    protected string SelectedStatus = "";
    protected string OtherPurpose = "";

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;

        await GetPurposes();

        IsLoading = false;
    }
    protected override void OnInitialized()
    {
        IsLoading = true;

        GetDropdownValues();
        BreadcrumbItems.AddRange(new List<BreadcrumbModel>
            {
                new BreadcrumbModel
                {
                    Icon = "menu",
                    Text = "Report",
                    Url = "/request-management",
                },
                new BreadcrumbModel
                {
                    Icon = "create_new_folder",
                    Text = $"Request Management Report",
                    Url = NavManager.Uri.ToString(),
                },
            });

        IsLoading = false;
    }
    protected async Task ShowReport(string Value,DateTime? start, DateTime? end)
    {
        IsLoading = true;

        SearchValue = Value;
        var filters = new List<Filter>();
   
        AddTextSearchFilter(filters, nameof(AuditLogModel.Created), start.Value.ToString(), "gte");
        AddTextSearchFilter(filters, nameof(AuditLogModel.Created), end.Value.ToString(), "lte");

        SearchFilterRequest.Filters = filters;
        GetFilterRequests();
        ServiceCb = AuditLog.AuditQuery;

        await LoadData();

        IsLoading = false;
    }
    protected async Task GetPurposes()
    {
        var res = await LookupsService.GetPurposes();
        if (res != null && res.Data.Count > 0)
        {
            Purposes = res.Data.ToList();
        }
    }
    protected void GetDropdownValues()
    {
        ReportType = ConfigManager.ReportType;
    }
    protected async Task ConfirmToExcel()
    {
        IsLoading = true;

        await ExceptionHandlerService.HandleApiException(async () =>
        {
            var fileName = $"Audit Trail {SelectedType} Type Report as of {DateTime.Now.ToString("MMM dd, yyyy")}.xlsx";
            var items = await GetAllItems();
            var selected = SelectedType == "Inventory" ? true : false;
            await ExceptionHandlerService.HandleApiException(async () => await AuditLogExportService.ExportList(selected, items, fileName), null);

        });

        IsLoading = false;
    }
    protected async Task<List<AuditLogModel>> GetAllItems()
    {
        var items = await GenericHelper.GetListByQuery<AuditLogModel>(
            new DataSourceRequest() { Skip = 0, Filter = DataSourceReq.Filter },
            AuditLog.AuditQuery,
            (err) => ToastService.ShowError(err)
        );

        return items;
    }
    public void OnDateRangePickerPopupClose(DateRangePickerCloseEventArgs args)
    {
        if (dateTo < dateFrom)
        {
            args.IsCancelled = true;
        }
    }
}
