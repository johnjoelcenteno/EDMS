using DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Telerik.FontIcons;
namespace DPWH.EDMS.Web.Client.Pages.UserManagement;

public class UserManagementBase : GridBase<UserModel>
{
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required ILicensesService LicensesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }

    protected double LicenseLimit = 0;
    protected double LicenseUsed = 0;
    protected double TotalUsers = 0;

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
                    LicenseUsed = licenseData.Limit - licenseData.Available;
                    LicenseLimit = licenseData.Limit;
                    TotalUsers = licenseData.EndUsersCount;
                }
            });
    }

    protected double GetLicenseAccumulatedPercentage()
    {
        return Math.Round((LicenseUsed / LicenseLimit) * 100, 2);
    }

}
