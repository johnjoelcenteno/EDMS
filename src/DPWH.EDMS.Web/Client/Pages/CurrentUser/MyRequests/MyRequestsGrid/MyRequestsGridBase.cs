using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.IDP.Core.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;


namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests.MyRequestsGrid;

public class MyRequestsGridBase : GridBase<RecordRequestModel>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }
    protected bool IsEndUser = false;

    protected async override Task OnInitializedAsync()
    {
        IsEndUser = await CheckIfEndUser();
        ServiceCb = RecordRequestsService.Query;
        await LoadData();
    }
    protected async Task<bool> CheckIfEndUser()
    {

        var authState = await AuthenticationStateAsync!;
        var user = authState.User;

        return (user.Identity is not null && user.Identity.IsAuthenticated) && user.IsInRole(ApplicationRoles.EndUser);
    }

    protected void GoToAddNewRequest()
    {
        NavManager.NavigateTo("my-requests/add");
    }

    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        IsLoading = true;

        var selectedItem = args.Item as RecordRequestModel;

        if (selectedItem != null)
        {
            NavManager.NavigateTo("my-requests/view/" + selectedItem.Id.ToString());
        }

        IsLoading = false;
    }
}