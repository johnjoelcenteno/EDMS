using AutoMapper;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DPWH.EDMS.Web.Client.Pages.UserProfile;

public class UserProfileBase : RxBaseComponent
{
    protected UserModel CurrentUser { get; set; } = new();
    protected string Role = string.Empty;

    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }
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
        IsLoading = true;
        await GetUser();
        IsLoading = false;
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
            var roleValue = user.Claims.FirstOrDefault(c => c.Type == "role")!.Value;
            var userRes = await UserService.GetById(Guid.Parse(userId));
            Role = GetRoleLabel(roleValue);
            if (userRes.Success)
            {
                CurrentUser = Mapper.Map<UserModel>(userRes.Data);
            }
        }
    }
    private string GetRoleLabel(string roleValue)
    {
        return ApplicationRoles.GetDisplayRoleName(roleValue, "Unknown Role");
    }
}
