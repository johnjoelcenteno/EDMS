using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records;

public class RecordsBase : GridBase<PersonalRecordDocument>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Parameter] public required string Id { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    protected List<PersonalRecordDocument> Record { get; set; } = new List<PersonalRecordDocument>();
    protected MockCurrentData CurrentData { get; set; }
    protected List<Document> DocumentList = new List<Document>();
    protected GetLookupResultIEnumerableBaseApiResponse GetEmployeeRecords { get; set; } = new GetLookupResultIEnumerableBaseApiResponse();

    protected string DisplayName = "---";
    protected string Role = string.Empty;
    protected override void OnInitialized()
    { 
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Records",
            Url = "/my-records"
        });

        Record = MockCurrentData.GenerateCurrentDocuments();
       
    }

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;

        await GetDocumentRecords();
        await GetUserInfo();

        IsLoading = false;
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
            DisplayName = !string.IsNullOrEmpty(user.Identity.Name) ? GenericHelper.CapitalizeFirstLetter(user.Identity.Name) : "---";
            Role = GetRoleLabel(roleValue);
           
        }
    }

    private string GetRoleLabel(string roleValue)
    {
        return ApplicationRoles.GetDisplayRoleName(roleValue, "Unknown Role");
    }

    protected async Task GetDocumentRecords()
    {
        IsLoading = true;

        var res = await LookupsService.GetEmployeeDocuments();
        if (res.Success)
        {
            GetEmployeeRecords = res;
        }

        IsLoading = false;
    }
    public async Task viewData(GridCommandEventArgs args)
    {
        PersonalRecordDocument selectedId = args.Item as PersonalRecordDocument;
        
        //Int32.TryParse(samp, out sampNumber);
        NavigationManager.NavigateTo($"/my-records/{selectedId.Id}");
    }
}
