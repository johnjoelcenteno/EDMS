using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.DataSource;


namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement;

public class RecordsManagementBase : GridBase<UserModel>
{
    [Inject] public required IUsersService UsersService { get; set; }
    public FilterOperator filterOperator { get; set; } = FilterOperator.StartsWith;


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
