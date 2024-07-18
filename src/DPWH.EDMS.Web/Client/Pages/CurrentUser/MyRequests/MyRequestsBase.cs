using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.Grid;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using DPWH.EDMS.IDP.Core.Constants;
using System.Security.Claims;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Components.Helpers;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests;

public class MyRequestsBase : RecordRequestGridComponentBase
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    protected bool IsEndUser = false;
    protected DateTime? SelectedDate { get; set; }
    protected int? SearchControlNumber { get; set; }
    protected string? SearchFullName { get; set; }
    protected string? SearchPurpose { get; set; }
    protected string? SearchStatus { get; set; }

    protected List<string> StatusList = new List<string>
    {
        "Review",
        "Release",
        "Claimed"
    };

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        await FetchUser();
        await HandleOnLoadGrid();
        IsLoading = false;
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

    protected async void SetFilterGrid()
    {
        var filters = new List<Api.Contracts.Filter>();

        AddDateFilter(filters);
        AddTextSearchFilterIfNotNull(filters, nameof(RecordRequestModel.ControlNumber), SearchControlNumber?.ToString(), "eq");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordRequestModel.FullName), SearchFullName, "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordRequestModel.Status), SearchStatus, "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordRequestModel.Purpose), SearchPurpose, "contains");

        SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
        SearchFilterRequest.Filters = filters.Any() ? filters : null;

        await LoadData();
        StateHasChanged();
    }

    private void AddDateFilter(List<Api.Contracts.Filter> filters)
    {
        if (SelectedDate.HasValue)
        {
            AddTextSearchFilter(filters, nameof(RecordRequestModel.DateRequested), SelectedDate.Value.ToString(), "gte");
            AddTextSearchFilter(filters, nameof(RecordRequestModel.DateRequested), SelectedDate.Value.AddDays(1).ToString(), "lte");
        }
    }

    private void AddTextSearchFilterIfNotNull(List<Api.Contracts.Filter> filters, string fieldName, string? value, string operation)
    {
        if (!string.IsNullOrEmpty(value))
        {
            AddTextSearchFilter(filters, fieldName, value, operation);
        }
    }

}