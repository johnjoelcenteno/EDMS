using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.Enums;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement;

public class RequestManagementBase : GridBase<RecordRequestModel>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }

    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }
    protected int ActiveTabIndex { get; set; } = 1;
    protected List<string> RequestStates = new List<string>();

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Request Management",
            Url = NavManager.Uri.ToString(),
        });

        LoadRequestStates();
    }

    protected async override Task OnInitializedAsync()
    {
        ServiceCb = RecordRequestsService.Query;
        await LoadData();
    }

    protected void GoToAddNewRequest()
    {
        NavManager.NavigateTo("request-management/add");
    }
    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        IsLoading = true;

        var selectedItem = args.Item as RecordRequestModel;

        if (selectedItem != null)
        {
            NavManager.NavigateTo("request-management/view/" + selectedItem.Id.ToString());
        }

        IsLoading = false;
    }
    protected void LoadRequestStates()
    {
        RequestStates = Enum.GetValues(typeof(RecordRequestStates))
               .Cast<RecordRequestStates>()
               .Select(e => e.ToString())
               .ToList();
    }

    protected async Task TabChangedHandler(int newIndex)
    {
        ActiveTabIndex = newIndex;
        var filters = new List<Api.Contracts.Filter>();
        string status = Enum.GetName(typeof(EDMS.Client.Shared.Enums.RecordRequestStates), ActiveTabIndex)!;

        if (ActiveTabIndex != 0 && !string.IsNullOrEmpty(status))
        {
            AddTextSearchFilter(filters, nameof(RecordRequestModel.Status), status);
        }

        // Set the filters
        SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
        SearchFilterRequest.Filters = filters;

        // Load data with the updated filters
        await LoadData();
        StateHasChanged();
    }
}
