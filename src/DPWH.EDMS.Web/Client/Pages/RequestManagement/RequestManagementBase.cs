using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.Grid;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement;

public class RequestManagementBase : RecordRequestGridComponentBase
{
    protected DateTime? SelectedDate { get; set; }
    protected int? SearchControlNumber { get; set; }
    protected string? SearchFullName { get; set; }
    protected string? SearchPurpose { get; set; }
    protected string? SearchStatus { get; set; }
    protected List<string> StatusList = new List<string>
    {
        "Submitted",
        "Reviewed",
        "Approved",
        "Released",
        "Claimed"
    };
    protected async override Task OnInitializedAsync()
    {
        await HandleOnLoadGrid();
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
