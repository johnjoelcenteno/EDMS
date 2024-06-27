using DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using DPWH.EDMS.Api.Contracts;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Telerik.FontIcons;
using DPWH.EDMS.Client.Shared.Models;
using Telerik.Blazor.Components;
using DPWH.EDMS.Client.Shared.APIClient.Services.DpwhIntegrations;

namespace DPWH.EDMS.Web.Client.Pages.UserManagement;

public class UserManagementBase : GridBase<UserModel>
{
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required ILicensesService LicensesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IDpwhIntegrationsService DpwhIntegrationService { get; set; }

    protected double LicenseLimit = 0;
    protected double LicenseUsed = 0;
    protected double TotalUsers = 0;
    protected string getOpenBtn = ""; 

    protected ICollection<GetRequestingOfficeResult> RegionOfficeList { get; set; } = new List<GetRequestingOfficeResult>();
    protected List<GetRequestingOfficeResultItem> DEOlist { get; set; } = new List<GetRequestingOfficeResultItem>();

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "group",
            Text = "User Management",
            Url = "/user-management"
        });
    }
    protected override async Task OnInitializedAsync()
    {
        ServiceCb = UserService.Query;
        await LoadData();

        await ExceptionHandlerService.HandleApiException(
            async () =>
            {
                var licenseRes = await LicensesService.GetLicenseStatus();

                if (licenseRes.Success)
                {
                    var licenseData = licenseRes.Data;
                    //LicenseUsed = licenseData.Limit - licenseData.Available;
                    LicenseUsed = 1; //Temporary Value for used license
                    LicenseLimit = licenseData.Limit;
                    TotalUsers = licenseData.EndUsersCount;
                }
            });
    } 
    protected double GetLicenseAccumulatedPercentage()
    {
        return Math.Round((LicenseUsed / LicenseLimit) * 100, 2);
    }
    protected async Task AddUser()
    {
            NavManager.NavigateTo("user-management/add");
    }
}
