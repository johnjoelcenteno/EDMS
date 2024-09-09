using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.AuditLog;
using DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.AuditTrailGridBase;
using DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.Model;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Web.Client.Shared.Services.Export.ExportAuditTrail;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Telerik.Blazor.Components; 

namespace DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.UserActivity
{
    public class UserActivityBase : AuditTrailsGridBase<AuditLogModel>
    {
        [Inject] public required IAuditLogService AuditLog { get; set; }
        [Inject] public required IAuditTrailExportService AuditLogExportService { get; set; }
        [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
        [Inject] public required IRequestManagementService RequestManagementService { get; set; }
        [Inject] public required IToastService ToastService { get; set; }     
        [Inject] public required NavigationManager NavManager { get; set; }
        protected DateTime dateFrom { get; set; } = DateTime.Now.AddDays(-1);
        protected DateTime dateTo { get; set; } = DateTime.Now;
        protected string SelectedType { get; set; } = "User";
        protected bool XSmall { get; set; }
        protected List<int?> PageSizes { get; set; } = new List<int?> { 5, 10, 15 };
        protected List<UserActivityModel> ReportType { get; set; } = new();
        protected UserActivityModel UserActivityData = new UserActivityModel();
        protected string SelectedReportType { get; set; }
        protected override void OnInitialized()
        {
            IsLoading = true;

            GetDropdownValues();

            IsLoading = false;
        }
        protected async Task ShowReport(string Value, DateTime? start, DateTime? end)
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
        protected void GetDropdownValues()
        {
            ReportType = new List<UserActivityModel>
            {
                new UserActivityModel { ReportTypeId = "RecordsManagement", ReportTypeName = "Records Management" },
                new UserActivityModel { ReportTypeId = "RequestsManagement", ReportTypeName = "Requests Management" },
                new UserActivityModel { ReportTypeId = "UserManagement", ReportTypeName = "User Management" },
                new UserActivityModel { ReportTypeId = "DataLibrary", ReportTypeName = "Data Library" }
            };
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
}
