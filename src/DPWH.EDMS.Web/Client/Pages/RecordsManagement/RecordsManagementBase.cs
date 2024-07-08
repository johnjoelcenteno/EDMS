using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.DataSource;


namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement;

public class RecordsManagementBase : GridBase<UserModel>
{
    [Inject] public required IUsersService UsersService { get; set; }
    public FilterOperator filterOperator { get; set; } = FilterOperator.StartsWith;
    protected string TextSearchFirstName = string.Empty;
    protected string TextSearcLastName = string.Empty;
    protected string TextSearchMiddlename = string.Empty;
    protected string TextSearchRegionalOffice = string.Empty;
    protected string TextSearchDepartment = string.Empty;

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Records Management",
            Url = "/records-management"
        });
    }

    protected async override Task OnInitializedAsync()
    {
        ServiceCb = UsersService.Query;
        await LoadData();
    }
    protected async Task SearchFilter()
    {
        var filters = new List<Filter>();

        // Add the search term filter
        if (!string.IsNullOrEmpty(TextSearchFirstName))
        {
            AddTextSearchFilter(filters, nameof(UserModel.FirstName), TextSearchFirstName.ToLower());
        }
        if (!string.IsNullOrEmpty(TextSearcLastName))
        {
            AddTextSearchFilter(filters, nameof(UserModel.LastName), TextSearcLastName.ToLower());
        }
        if (!string.IsNullOrEmpty(TextSearchMiddlename))
        {
            AddTextSearchFilter(filters, nameof(UserModel.MiddleInitial), TextSearchMiddlename.ToLower());
        }
        if (!string.IsNullOrEmpty(TextSearchRegionalOffice))
        {
            AddTextSearchFilter(filters, nameof(UserModel.RegionalOfficeRegion), TextSearchRegionalOffice.ToLower());
        }
        if (!string.IsNullOrEmpty(TextSearchDepartment))
        {
            AddTextSearchFilter(filters, nameof(UserModel.Department), TextSearchDepartment.ToLower());
        }

        // Set the logic for combining the filters (OR logic in this case)
        SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;

        // Set the filters
        SearchFilterRequest.Filters = filters;

        // Load data with the updated filters
        await LoadData();
    }

    protected void GoToSelectedItemDocuments(GridRowClickEventArgs args)
    {
        IsLoading = true;

        var selectedItem = args.Item as UserModel;

        if (selectedItem != null)
        {
            NavManager.NavigateTo("records-management/" + selectedItem.Id.ToString());
        }

        IsLoading = false;
    }
}
