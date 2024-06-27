using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Enums;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests.GridBaseExtension;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests;

public class MyRequestsBase : MyRequestGridBase
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }

    protected bool IsEndUser = false;
    protected int ActiveTabIndex { get; set; } = 1;
    protected List<string> RequestStates = new List<string>();


    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Requests",
            Url = NavManager.Uri.ToString(),
        });

        LoadRequestStates();
    }

    protected async override Task OnInitializedAsync()
    {
        await LoadUserConfigsAndGrid();

    }

    protected void LoadRequestStates()
    {
        RequestStates = Enum.GetValues(typeof(RecordRequestStates))
               .Cast<RecordRequestStates>()
               .Select(e => e.ToString())
               .ToList();
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

    protected async Task TabChangedHandler(int newIndex)
    {
        IsLoading = true;
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