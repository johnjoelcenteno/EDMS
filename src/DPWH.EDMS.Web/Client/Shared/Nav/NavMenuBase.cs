using DPWH.EDMS.Web.Client.Shared.Services.Navigation;
using DPWH.EDMS.Web.Shared.Configurations;
using DPWH.EDMS.Web.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Shared.Nav;

public class NavMenuBase: RxBaseComponent
{
    //[CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    //[Inject] public required AuthRxService AuthRxService { get; set; }
    [Inject] public required ConfigManager ConfigManager { get; set; }
    [Inject] public required IMenuDataService MenuDataService { get; set; }
    [Inject] public required NavRx NavRx { get; set; }
    [Inject] public required NavigationManager NavManager { get; set; }

    protected bool IsNavMenuCollapsed = false;
    protected TelerikDrawer<MenuModel> DrawerRef = new();
    protected List<MenuModel> NavMenus { get; set; } = new List<MenuModel>();
    //protected bool XSmall { get; set; }
    //protected bool Small { get; set; }
    //protected bool Medium { get; set; }
    //protected bool Large { get; set; }
    protected List<IDisposable> RxSubscriptions { get; set; } = new();

    protected string DisplayName = string.Empty;
    protected string RoleTitle = string.Empty;

    protected MenuModel? SelectedLevel1Item = null;

    //protected readonly string LoginUrl = "bff/login";

    protected override async Task OnInitializedAsync()
    {
        //RxSubscriptions.Add(
        //    AuthRxService.IsLoggedIn.Subscribe(async loginSuccess =>
        //    {
        //        if (loginSuccess)
        //        {
        //            await SetMenu();
        //            StateHasChanged();
        //        }
        //    }
        //));

        await SetMenu();
    }

    private async Task SetMenu()
    {
        //if (AuthenticationStateAsync is null)
        //    return;

        //var menus = MenuDataService.GetMenuItems();

        //var authState = await AuthenticationStateAsync;
        //var user = authState.User;

        //if (user.Identity is not null && user.Identity.IsAuthenticated)
        //{
        //    DisplayName = user.GetFullName();
        //    var roles = GetRoles(user);
        //    NavMenus = menus.Where(x => x.AuthorizedRoles.Any(role => roles.Contains(role))).ToList();
        //}

        NavMenus = MenuDataService.GetMenuItems().ToList();
    }
    //private IList<string> GetRoles(ClaimsPrincipal claimsPrincipal)
    //{
    //    if (claimsPrincipal.IsClient())
    //    {
    //        RoleTitle = RoleConstants.CorporateRole;
    //        return new List<string> { RoleConstants.CorporateRole };
    //    }
    //    else
    //    {
    //        var _roles = claimsPrincipal.GetRoles();
    //        if (_roles != null && _roles.Any())
    //        {
    //            if (_roles.Any(role => BookingPlatformRoles.Policy.AdminOnly.Any(x => x == role)))
    //            {
    //                RoleTitle = RoleConstants.Admin;
    //                return new List<string> { RoleConstants.Admin, RoleConstants.SalesRole };
    //            }
    //            else if (_roles.Any(role => role == BookingPlatformRoles.Sales))
    //            {
    //                RoleTitle = RoleConstants.SalesRole;
    //                return new List<string> { RoleConstants.SalesRole };
    //            }
    //        }
    //    }
    //    return new List<string>();
    //}

    protected async Task ToggleDrawer()
    {
        await DrawerRef.ToggleAsync();
        NavRx.IsExpanded.OnNext(DrawerRef.Expanded);
        StateHasChanged();
    }

    protected void ToggleMenuItem(MenuModel item) => item.Expanded = !item.Expanded;

    protected void ToggleLevel1Item(MenuModel item)
    {
        SelectedLevel1Item = item;
        StateHasChanged();
    }
}
