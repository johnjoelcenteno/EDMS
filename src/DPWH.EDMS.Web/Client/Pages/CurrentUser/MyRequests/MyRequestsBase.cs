using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.Grid;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using DPWH.EDMS.IDP.Core.Constants;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests;

public class MyRequestsBase : RecordRequestGridComponentBase
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    protected bool IsEndUser = false;

    protected async override Task OnInitializedAsync()
    {
        await FetchUser();
        await HandleOnLoadGrid();
    }

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Requests",
            Url = NavManager.Uri.ToString(),
        });        
    }

    protected bool CheckIfEndUser(ClaimsPrincipal user)
    {
        return (user.Identity is not null && user.Identity.IsAuthenticated) && user.IsInRole(ApplicationRoles.EndUser);
    }

    protected async Task FetchUser()
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
        }
        else
        {
            ToastService.ShowError("Something went wrong on fetching user data");
        }
        IsLoading = false;
    }

    protected void GoToAddNewRequest()
    {
        HandleGoToAddNewRequest("my-requests/add");
    }
    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        HandleSelectedItemOverview(args, "my-requests/view/");
    }
}