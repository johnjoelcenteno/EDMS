using DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using DPWH.EDMS.Api.Contracts;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Telerik.FontIcons;
using Telerik.Blazor.Components;
using DPWH.EDMS.Client.Shared.APIClient.Services.DpwhIntegrations;
using Telerik.Blazor.Components.Menu;
using DPWH.EDMS.IDP.Core.Constants;
using Microsoft.AspNetCore.Components.Web;
using DPWH.EDMS.Components.Helpers;

namespace DPWH.EDMS.Web.Client.Pages.UserManagement;

public class UserManagementBase : GridBase<UserModel>
{
    [Inject] protected NavigationManager Nav { get; set; } = default!;
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required ILicensesService LicensesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IDpwhIntegrationsService DpwhIntegrationService { get; set; }

    protected double LicenseLimit = 0;
    protected double LicenseUsed = 0;
    protected double TotalUsers = 0;
    protected string GetOpenBtn = "";
    protected string SelectedAcord { get; set; }
    protected UserModel SelectedItem { get; set; } = default!;
    protected UserModel UserList { get; set; } = new UserModel();
    protected ICollection<GetRequestingOfficeResult> RegionOfficeList { get; set; } = new List<GetRequestingOfficeResult>();
    protected List<GetRequestingOfficeResultItem> DEOlist { get; set; } = new List<GetRequestingOfficeResultItem>();
    protected List<GridMenuItemModel> MenuItems { get; set; } = new();
    protected TelerikContextMenu<GridMenuItemModel> ContextMenuRef { get; set; } = new();
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "group",
            Text = "User Management",
            Url = "/user-management"
        });
    }
    protected void GetGridMenuItems()
    {
        // context menu items
        MenuItems = new List<GridMenuItemModel>
        {
        new (){ Text = "View", Icon=null!, CommandName="View" },
        new (){ Text = "Edit", Icon=null!, CommandName="Edit" }
        };
    }
    protected async Task ShowRowOptions(MouseEventArgs e, UserModel row)
    {
        SelectedItem = row;

        await ContextMenuRef.ShowAsync(e.ClientX, e.ClientY);
    }
    protected override async Task OnInitializedAsync()
    {
        //ServiceCb = UserService.Query;
        //await LoadData();
        await LoadUserData();
        GetGridMenuItems();
        await ExceptionHandlerService.HandleApiException(
            async () =>
            {
                var licenseRes = await LicensesService.GetLicenseStatus();

                if (licenseRes.Success)
                {
                    var licenseData = licenseRes.Data;
                    LicenseUsed = licenseData.Limit - licenseData.Available;
                    //LicenseUsed = 1; //Temporary Value for used license
                    LicenseLimit = licenseData.Limit;
                    TotalUsers = licenseData.EndUsersCount;
                }
            });
    }

    protected async Task LoadUserData()
    {
        var result = await UserService.Query(DataSourceReq);
        if (result.Data != null)
        {
            var getData = GenericHelper.GetListByDataSource<UserModel>(result.Data);
            GridData = getData;
        }
       
    }
    protected double GetLicenseAccumulatedPercentage()
    {
        return Math.Round((LicenseUsed / LicenseLimit) * 100, 2);
    }
    protected async Task AddUser()
    {
        NavManager.NavigateTo("user-management/add");
    }
    protected void OnItemClick(GridMenuItemModel item)
    {
        // one way to pass handlers is to use an Action, you don't have to use this
        if (item.Action != null)
        {
            item.Action.Invoke();
        }
        else
        {
            // or you can use local code to perform a task
            // such as put a row in edit mode or select it
            SelectedAcord = item.CommandName;
            var guid = Guid.Parse(SelectedItem.Id);
            switch (item.CommandName)
            {

                case "View":
                    //var viewUrl = $"{Nav.BaseUri}user-management/view-layout/{SelectedItem.Id}";
                    //Nav.NavigateTo(viewUrl);
                    break;
                case "Update":
                    //Nav.NavigateTo($"{Nav.BaseUri}user-management/edit/{Id}");
                    break;

                default:
                    break;
            }
        }

    }
}
