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
    public UserModel currentUser;

    [Inject] public required IUsersService UserService { get; set; }
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }

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

        var authState = await AuthenticationStateAsync;
        var user = authState.User;
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var userId = user.Claims.FirstOrDefault(c => c.Type == "sub")!.Value;
            var userRes = await UserService.GetById(Guid.Parse(userId));
            if (userRes.Success)
            {

                this.currentUser = new UserModel
                {
                    Id = userRes.Data.Id,
                    UserName = userRes.Data.UserName,
                    Email = userRes.Data.Email,
                    FirstName = userRes.Data.FirstName,
                    MiddleInitial = userRes.Data.MiddleInitial,
                    LastName = userRes.Data.LastName,
                    MobileNumber = userRes.Data.MobileNumber,
                    EmployeeId = "---",
                    Role = userRes.Data.Role,
                    UserAccess = userRes.Data.UserAccess,
                    Department = userRes.Data.Department,
                    Position = userRes.Data.Position,
                    RegionalOfficeRegion = userRes.Data.RegionalOfficeRegion,
                    RegionalOfficeProvince = userRes.Data.RegionalOfficeProvince,
                    DistrictEngineeringOffice = userRes.Data.DistrictEngineeringOffice,
                    DesignationTitle = userRes.Data.DesignationTitle,
                    CreatedBy = userRes.Data.CreatedBy,
                    Created = userRes.Data.CreatedDate,
                };

            }
        }
    }
}
