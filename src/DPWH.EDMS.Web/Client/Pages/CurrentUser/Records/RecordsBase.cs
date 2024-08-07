using DocumentFormat.OpenXml.InkML;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordManagement;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.Common.Model;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Common.Trees.Models;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records;

public class RecordsBase : GridBase<LookupRecordModels>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Parameter] public required string Id { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    [Inject] public required IRecordManagementService RecordManagementService { get; set; }
    protected EDMS.Client.Shared.MockModels.RecordModel Record { get; set; }
    protected MockCurrentData CurrentData { get; set; }
    protected List<LookupRecordModels> GetRecordType = new List<LookupRecordModels>();
    protected List<Document> DocumentList = new List<Document>();
    protected string? SearchDocVersion { get; set; }
    protected string? SearchName { get; set; }
    public IEnumerable<TreeItem> Data { get; set; }
    public IEnumerable<object> ExpandedItems { get; set; }
    protected GetLookupResultIEnumerableBaseApiResponse GetEmployeeRecords { get; set; } = new GetLookupResultIEnumerableBaseApiResponse();

    protected string DisplayName = "---";
    protected string Role = string.Empty;
    protected string EmployeeId;
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Records",
            Url = "/my-records"
        });

        Record = MockCurrentData.GetCurrentRecord();

    }

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        await GetUserInfo();
        await GetDocumentRecordsDetails();
        IsLoading = false;
    }

    public async Task OnRowClickHandler(GridRowClickEventArgs args)
    {
        var currItem = args.Item as LookupRecordModels;

        var state = GridRef.GetState();

        bool isCurrItemExpanded = state.ExpandedItems.Any(x => x.Id == currItem?.Id);

        if (isCurrItemExpanded)
        {
            state.ExpandedItems.Remove(currItem!);
        }
        else
        {
            state.ExpandedItems.Add(currItem!);
        }

        await GridRef.SetStateAsync(state);
    }
    private async Task GetUserInfo()
    {
        if (AuthenticationStateAsync is null)
            return;


        var authState = await AuthenticationStateAsync;
        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var roleValue = user.Claims.FirstOrDefault(c => c.Type == "role")!.Value;
            EmployeeId = user.Claims.FirstOrDefault(c => c.Type == "employee_number")!.Value;
            DisplayName = !string.IsNullOrEmpty(user.Identity.Name) ? GenericHelper.CapitalizeFirstLetter(user.Identity.Name) : "---";
            Role = GetRoleLabel(roleValue);

        }
    }

    private string GetRoleLabel(string roleValue)
    {
        return ApplicationRoles.GetDisplayRoleName(roleValue, "Unknown Role");
    }


    protected async Task GetDocumentRecordsDetails()
    {
        IsLoading = true;
        ServiceCb = RecordManagementService.Query;
        var filters = new List<Api.Contracts.Filter>();
        AddTextSearchFilterIfNotNull(filters, nameof(LookupRecordModels.EmployeeId), EmployeeId, "eq");
        SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
        SearchFilterRequest.Filters = filters.Any() ? filters : null;
        await LoadData();

        var res = await LookupsService.GetPersonalRecords();
        if (res.Success)
        {
            GetEmployeeRecords = res;
            var convertedData = GetEmployeeRecords.Data
                    .Select(item => new LookupRecordModels
                    {
                        Id = item.Id,
                        RecordName = item.Name,
                 
                     
                    }).ToList();
            GetRecordType = convertedData;
        }

        IsLoading = false;
    }

    public async Task viewData(GridCommandEventArgs args)
    {
        LookupRecordModels? selectedId = args.Item as LookupRecordModels;

        //Int32.TryParse(samp, out sampNumber);
        Console.WriteLine(selectedId?.Id);
        NavigationManager.NavigateTo($"/my-records/{selectedId.Id}");
    }
    //protected async void SetFilterGrid()
    //{
    //    var document = GetRecordType;
    //    var filters = new List<Api.Contracts.Filter>();

    //    AddTextSearchFilterIfNotNull(filters, nameof(LookupRecordModels.EmployeeId), EmployeeId?.ToString(), "eq");
    //    AddTextSearchFilterIfNotNull(filters, nameof(LookupRecordModels.Documents), SearchDocVersion, "contains");
    //    AddTextSearchFilterIfNotNull(filters, nameof(LookupRecordModels.RecordName), SearchName, "contains");

    //    SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
    //    SearchFilterRequest.Filters = filters.Any() ? filters : null;

    //    await LoadData();

    //    StateHasChanged();
    //}
    private void AddTextSearchFilterIfNotNull(List<Api.Contracts.Filter> filters, string fieldName, string? value, string operation)
    {
        if (!string.IsNullOrEmpty(value))
        {
            AddTextSearchFilter(filters, fieldName, value, operation);
        }
    }
}
