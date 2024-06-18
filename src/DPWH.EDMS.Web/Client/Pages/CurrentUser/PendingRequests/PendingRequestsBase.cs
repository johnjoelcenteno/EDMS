using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.IDP.Core.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.PendingRequests;

public class PendingRequestsBase : GridBase<RecordRequestModel>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }

    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }

    protected bool IsEndUser = false;
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Pending Request",
            Url = NavManager.Uri.ToString(),
        });        
    }

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
        NavManager.NavigateTo("my-pending-request/add");
    }
}
