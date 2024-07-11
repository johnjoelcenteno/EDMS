using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records;

public class RecordsBase : GridBase<GetLookupResult>
{
    [Parameter] public required string Id { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    protected EDMS.Client.Shared.MockModels.RecordModel Record { get; set; }
    protected MockCurrentData CurrentData { get; set; }
    protected List<Document> DocumentList = new List<Document>();
    protected GetLookupResultIEnumerableBaseApiResponse GetEmployeeRecords { get; set; } = new GetLookupResultIEnumerableBaseApiResponse();
    protected override void OnInitialized()
    { 
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Records",
            Url = "/my-records"
        });

        Record = MockCurrentData.GetCurrentRecord();
        DocumentList = MockCurrentData.GetDocuments().OrderBy(d => d.DocumentName).ToList();
         
    }
    protected async override Task OnInitializedAsync()
    {
        await GetDocumentRecords();
    }
    protected async Task GetDocumentRecords()
    {
        IsLoading = true;

        var res = await LookupsService.GetEmployeeRecords();
        if (res.Success)
        {
            GetEmployeeRecords = res;
        }

        IsLoading = false;
    }
    public async Task viewData(GridCommandEventArgs args)
    {
        GetLookupResult selectedId = args.Item as GetLookupResult;
        
        //Int32.TryParse(samp, out sampNumber);
        NavigationManager.NavigateTo($"/my-records/{selectedId.Id}");
    }
}
