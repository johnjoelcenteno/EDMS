using AutoMapper;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.Grid;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;
using Telerik.DataSource;


namespace DPWH.EDMS.Web.Client.Pages.RequestManagement;

public class RequestManagementBase : RecordRequestGridComponentBase
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }
    protected List<RecordRequestModel> RecordRequest { get; set; } = new();
    protected UserModel CurrentUser { get; set; } = new();
    protected DateTime? SelectedDate { get; set; }
    protected string Role = string.Empty;
    protected string Office = string.Empty;
    protected int? SearchControlNumber { get; set; }
    protected int? ForApprovalCount { get; set; } = 0;
    protected string? ForApproval { get; set; }
    protected string? SearchFullName { get; set; }
    protected string? SearchPurpose { get; set; }
    protected string? SearchRmdStatus { get; set; }
    protected string? SearchHrmdStatus { get; set; }
    protected string? SearchStatus { get; set; }
    protected List<string> StatusList = new List<string>
    {
        "Submitted",
        "Reviewed",
        "Approved",
        "Released",
        "Claimed"
    };
    protected List<string> OfficeStatusList = new List<string>
    {
        "NA",
        "Submitted",
        "Reviewed",
        "Approved",
        "Released",
        "Claimed"
    };
    protected async override Task OnInitializedAsync()
    {
        await GetUser();
        await HandleOnLoadGrid();

        var req = new DPWH.EDMS.Api.Contracts.DataSourceRequest
        {

            Filter = new Filter
            {
                Logic = "and",
                Filters = new List<Filter>
                {
                    new Filter
                    {
                        Logic = "and",
                        Filters = new List<Filter>
                        {
                            new Filter
                            {
                                Field = "RMDRequestStatus",
                                Operator = "isnotnull"
                            },
                            new Filter
                            {
                                Field = "RMDRequestStatus",
                                Operator = "eq",
                                Value = "Reviewed"
                            }
                        }
                    }
                }
            }
        };

        var res = await RequestManagementService.Query(req);
        ForApprovalCount = res.Total;
        ForApproval = $"For Approval {ForApprovalCount}";
    }

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Request Management",
            Url = NavManager.Uri.ToString(),
        });
    }

    private async Task GetUser()
    {
        if (AuthenticationStateAsync is null)
            return;

        var authState = await AuthenticationStateAsync;
        var user = authState.User;
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var roles = user.Claims.Where(c => c.Type == "role")!.ToList();
            var role = roles.FirstOrDefault(role => !string.IsNullOrEmpty(role.Value) && role.Value.Contains(ApplicationRoles.RolePrefix))?.Value ?? string.Empty;

            var userId = user.Claims.FirstOrDefault(c => c.Type == "sub")!.Value;

            var userRes = await UserService.GetById(Guid.Parse(userId));
            Role = GetRoleLabel(role);

            if (userRes.Success)
            {
                CurrentUser = Mapper.Map<UserModel>(userRes.Data);
            }

            var office = ClaimsPrincipalExtensions.GetOffice(user);
            Office = !string.IsNullOrEmpty(office) ? office : "---";
        }
    }

    private string GetRoleLabel(string role)
    {
        return ApplicationRoles.GetDisplayRoleName(role, "Unknown Role");
    }

    protected void GoToAddNewRequest()
    {
        HandleGoToAddNewRequest("request-management/add");
    }
    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        HandleSelectedItemOverview(args, "request-management/view-request-form/");
    }

    protected async void SetFilterGrid()
    {
        var filters = new List<Api.Contracts.Filter>();

        AddDateFilter(filters);
        AddTextSearchFilterIfNotNull(filters, nameof(RecordRequestModel.ControlNumber), SearchControlNumber?.ToString(), "eq");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordRequestModel.FullName), SearchFullName, "contains");
        AddTextSearchFilterIfNotNull(filters, "RMDRequestStatus", SearchRmdStatus, "contains");
        AddTextSearchFilterIfNotNull(filters, "HRMDRequestStatus", SearchHrmdStatus, "contains");
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
