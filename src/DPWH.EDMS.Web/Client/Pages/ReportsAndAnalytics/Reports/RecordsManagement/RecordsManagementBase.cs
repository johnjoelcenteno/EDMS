
using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.SystemReport;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Web.Client.Shared.Services.Export;
using Microsoft.AspNetCore.Components;
using Telerik.FontIcons;
using RecordModel = DPWH.EDMS.Client.Shared.MockModels.RecordModel;

namespace DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.Reports.RecordsManagement;

public class ReccordsManagementBase : GridBase<RecordModel>
{
    [Inject] public required IExcelExportService _ExcelExportService { get; set; }
    [Inject] public required ISystemReportService _SystemService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }

    protected readonly string TwoFormFieldsResponsiveClass = "col-12 col-md-6 col-lg-3";
    protected List<RecordModel> EmployeeList = new List<RecordModel>();
    protected List<Document> DocumentList = new List<Document>();
    protected bool IsShowing { get; set; } = false;
    protected bool Visible { get; set; } = false;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        DocumentList = MockData.GenerateDocuments().ToList();
        GridData = GenerateEmployeeRecords(5);
        BreadcrumbItems = new List<BreadcrumbModel>
                {
                    new() { Icon = FontIcon.Home.ToString(), Url = "/" },
                    new() { Text = "Reports", Icon = FontIcon.Menu.ToString() },
                    new() { Text = "Record Management", Icon = FontIcon.TableUnmerge.ToString() }
                };
    }
    protected override async Task OnInitializedAsync()
    {
        //await ShowUserReport();
    }

    protected async Task DownloadToExcel()
    {
        Visible = !Visible;

    }

    protected async Task ConfirmToExcel()
    {
        Visible = false;
        IsLoading = true;

        var fileName = $"System Reports as of {DateTime.Now.ToString("MMM dd, yyyy")}.xlsx";
        await ExceptionHandlerService.HandleApiException(async () => await _ExcelExportService.ExportList(GridData, fileName), null);


        IsLoading = false;
    }

    protected async Task ShowUserReport()
    {
        IsLoading = true;
        await ExceptionHandlerService.HandleApiException(async () =>
        {
            ServiceCb = _SystemService.QuerySystemLogs;
            await LoadData();

        });

        IsLoading = false;
    }
    private List<RecordModel> GenerateEmployeeRecords(int count)
    {
        var employees = new List<RecordModel>();
        var random = new Random();
        for (int i = 1; i <= count; i++)
        {
            var employee = new RecordModel
            {
                Office = random.Next(0, 2) == 0 ? "RMD" : "HRMD",
                LastName = random.Next(0, 2) == 0 ? "Current" : "Non-current",
                FirstName = "Document" + i
            };

            employees.Add(employee);
        }

        return employees;
    }
}


