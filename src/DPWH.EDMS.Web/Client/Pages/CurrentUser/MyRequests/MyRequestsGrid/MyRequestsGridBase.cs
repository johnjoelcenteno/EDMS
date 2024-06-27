using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests.GridBaseExtension;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Telerik.Blazor.Components;


namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests.MyRequestsGrid;

public class MyRequestsGridBase : MyRequestGridBase
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }

    protected bool IsEndUser = false;

    protected async override Task OnInitializedAsync()
    {
        await LoadUserConfigsAndGrid();

    }
    protected bool CheckIfEndUser(ClaimsPrincipal user)
    {
        return (user.Identity is not null && user.Identity.IsAuthenticated) && user.IsInRole(ApplicationRoles.EndUser);
    }

    protected async Task LoadUserConfigsAndGrid()
    {
        IsLoading = true;
        var authState = await AuthenticationStateAsync!;
        var user = authState.User;

        var userId = user.GetUserId();

        IsEndUser = CheckIfEndUser(user);

        var userRes = await UsersService.GetById(userId);

        if (userRes.Success)
        {
            EmployeeId = userRes.Data.EmployeeId;
            await LoadData(); // Load Grid
        }
        else
        {
            ToastService.ShowError("Something went wrong on fetching user data");
        }
        IsLoading = false;
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