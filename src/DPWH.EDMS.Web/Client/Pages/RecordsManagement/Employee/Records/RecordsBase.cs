using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Web.Client.Shared.Records.Grid;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Web.Client.Pages.RecordsManagement.Employee.Records.Model;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordManagement;
using DPWH.EDMS.Components.Helpers;
using Microsoft.JSInterop;
using Telerik.SvgIcons;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.Employee.Records;

public class RecordsBase : GridBase<GetLookupResult>
{
    [Parameter] public required GetUserByIdResult SelectedEmployee { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    [Inject] protected IJSRuntime? JS { get; set; }
    [Inject] public required IRecordManagementService RecordManagementService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    protected GetLookupResultIEnumerableBaseApiResponse GetEmployeeRecords { get; set; } = new GetLookupResultIEnumerableBaseApiResponse();
    protected List<RecordDocumentModel> RecordDocuments { get; set; } = new();
    protected int pageAction = 5;
    protected List<int?> PageSizesChild { get; set; } = new List<int?> { 5, 10, 15 };
    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;

        await GetDocumentRecords();

        IsLoading = false;
    }
    protected async Task GetDocumentRecords()
    {
        IsLoading = true;

        var res = await LookupsService.GetPersonalRecords();
        if (res.Success)
        {
            GetEmployeeRecords = res;
        }

        IsLoading = false;
    }
    protected override async Task OnParametersSetAsync()
    {
        var recordResult = await RecordManagementService.QueryByEmployeeId(SelectedEmployee.EmployeeId, DataSourceReq);
        if (recordResult.Data != null) 
        {
            RecordDocuments = GenericHelper.GetListByDataSource<RecordDocumentModel>(recordResult.Data);
        }
    }
    protected async Task DownloadFromStream(string uri, string name)
    {
        var fileUri = uri;
        var fileStream = await GetFileStreamFromUri(fileUri);
        var fileName = name;

        using var streamRef = new DotNetStreamReference(stream: fileStream);
        await JS!.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }
    protected async Task<Stream> GetFileStreamFromUri(string fileUri)
    {
        using var httpClient = new HttpClient();
        var fileBytes = await httpClient.GetByteArrayAsync(fileUri);
        var fileStream = new MemoryStream(fileBytes);
        return fileStream;
    }
 
    protected override void OnInitialized()
    {
        // ----
    }

    //protected void GoToAddNewRequest()
    //{
    //    HandleGoToAddNewRequest("record-management/request-history/add/" + SelectedEmployee.Id);
    //}
    protected void GoToAddNewRecord()
    {
       // NavigationManager.NavigateTo("record-management/records/add/" + SelectedEmployee.Id);
    }
    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        //HandleSelectedItemOverview(args, "request-management/view-request-form/");
    }
}
