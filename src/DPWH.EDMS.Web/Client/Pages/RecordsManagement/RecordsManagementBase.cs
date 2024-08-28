using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
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
    protected List<UserModel> UserRecords = new List<UserModel>();
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
        IsLoading = true;

        await LoadRecordsManagementData();

        IsLoading = false;
    }
    protected async Task LoadRecordsManagementData()
    {
        var res = await UsersService.Query(DataSourceReq);

        if(res.Data != null)
        {
            var getData = GenericHelper.GetListByDataSource<UserModel>(res.Data);
            UserRecords = getData;
        }
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
