using DPWH.EDMS.Components;
using DPWH.EDMS.Client.Shared.Models;
using Telerik.FontIcons;
using DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Components.Helpers;
using static System.Net.Mime.MediaTypeNames;

namespace DPWH.EDMS.Web.Client.Pages.UserProfile;

public class UserProfileBase : RxBaseComponent
{
    [Inject] public required IUsersService UserService { get; set; }
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }

    protected List<UserModel> UserList = new List<UserModel>();

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "person",
            Text = "Profile",
            Url = "/profile"
        });
    }

    protected async override Task OnInitializedAsync()
    {
        await GetUser();
    }
    private async Task GetUser()
    {
        if (AuthenticationStateAsync is null)
            return;

        //var menus = MenuDataService.GetMenuItems();

        var authState = await AuthenticationStateAsync;
        var user = authState.User;
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var userId = user.Claims.FirstOrDefault(c => c.Type == "sub")!.Value;
            var userRes = await UserService.GetById(Guid.Parse(userId));
            var userEmployeeId = await UserService.GetUserByEmployeeId(userId);
            if (userRes.Success && userEmployeeId.Success)
            {

                var currentUser = new UserModel
                {
                    Id = GenericHelper.GetDisplayValue(userRes.Data.Id),
                    UserName = GenericHelper.GetDisplayValue(userRes.Data.UserName),
                    Email = GenericHelper.GetDisplayValue(userRes.Data.Email),
                    FirstName = GenericHelper.GetDisplayValue(userRes.Data.FirstName),
                    MiddleInitial = GenericHelper.GetDisplayValue(userRes.Data.MiddleInitial),
                    LastName = GenericHelper.GetDisplayValue(userRes.Data.LastName),
                    MobileNumber = GenericHelper.GetDisplayValue(userRes.Data.MobileNumber),
                    EmployeeId = GenericHelper.GetDisplayValue("---"),
                    Role = GenericHelper.GetDisplayValue(userRes.Data.Role),
                    UserAccess = GenericHelper.GetDisplayValue(userRes.Data.UserAccess),
                    Department = GenericHelper.GetDisplayValue(userRes.Data.Department),
                    Position = GenericHelper.GetDisplayValue(userRes.Data.Position),
                    RegionalOfficeRegion = GenericHelper.GetDisplayValue(userRes.Data.RegionalOfficeRegion),
                    RegionalOfficeProvince = GenericHelper.GetDisplayValue(userRes.Data.RegionalOfficeProvince),
                    DistrictEngineeringOffice = GenericHelper.GetDisplayValue(userRes.Data.DistrictEngineeringOffice),
                    DesignationTitle = GenericHelper.GetDisplayValue(userRes.Data.DesignationTitle),
                    CreatedBy = GenericHelper.GetDisplayValue(userRes.Data.CreatedBy),
                    Created = userRes.Data.CreatedDate,
                };
                UserList.Add(currentUser);
            }

            //DisplayName = !string.IsNullOrEmpty(user.Identity.Name) ? GenericHelper.CapitalizeFirstLetter(user.Identity.Name) : "---";
            //Role = GetRoleLabel(roleValue);
            //NavMenus = MenuDataService.GetMenuItems().Where(m => m.AuthorizedRoles.Any(r => r == roleValue)).ToList();
            //NavMenus2 = MenuDataService.GetMenuItems2().Where(m => m.AuthorizedRoles.Any(r => r == roleValue)).ToList();
            //NavSettings = MenuDataService.GetSettingsItems().Where(m => m.AuthorizedRoles.Any(r => r == roleValue)).ToList();
        }
    }
}
