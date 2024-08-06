using Blazored.Toast.Services;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.AuditLog;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.AuditTrailGridBase;
using DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.Model;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Web.Client.Shared.Services.Export.ExportAuditTrail;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.UserManagement;

public class UserManagementBase : AuditTrailsGridBase<AuditLogModel>
{
    [Inject] public required IAuditLogService AuditLog { get; set; }
    [Inject] public required IAuditTrailExportService AuditLogExportService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    protected DateTime dateFrom { get; set; } = DateTime.Now.AddDays(-1);
    protected DateTime dateTo { get; set; } = DateTime.Now;
    protected string SelectedType { get; set; } = "User";
    protected bool XSmall { get; set; }
    protected List<int?> PageSizes { get; set; } = new List<int?> { 5, 10, 15 };
    protected async Task ShowReport(string Value, DateTime? start, DateTime? end)
    {
        IsLoading = true;

        await ExceptionHandlerService.HandleApiException(async () => {

            SearchValue = Value;
            ServiceCb = AuditLog.AuditQuery;
            var filters = new List<Filter>();
            AddTextSearchFilter(filters, nameof(AuditLogModel.Created), start.Value.ToString(), "gte");
            AddTextSearchFilter(filters, nameof(AuditLogModel.Created), end.Value.ToString(), "lte");

            SearchFilterRequest.Filters = filters;
            await LoadData();
        });
        
        IsLoading = false;
    }
    protected async Task ConfirmToExcel()
    {
        IsLoading = true;

        await ExceptionHandlerService.HandleApiException(async () => {
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