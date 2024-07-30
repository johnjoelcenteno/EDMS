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
            var roles = user.Claims.Where(c => c.Type == "role")!.ToList();
            var role = roles.FirstOrDefault(role => !string.IsNullOrEmpty(role.Value) && role.Value.Contains(ApplicationRoles.RolePrefix))?.Value ?? string.Empty;

            var userId = user.Claims.FirstOrDefault(c => c.Type == "sub")!.Value;
             
            var userRes = await UserService.GetById(Guid.Parse(userId));
            Role = GetRoleLabel(role);

            if (userRes.Success)
            {
                CurrentUser = Mapper.Map<UserModel>(userRes.Data);
            }
        }
    }
    private string GetRoleLabel(string role)
    {
        return ApplicationRoles.GetDisplayRoleName(role, "Unknown Role");
    }
}
