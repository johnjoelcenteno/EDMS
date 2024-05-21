﻿using DPWH.EDMS.Client.Shared.Configurations;
using DPWH.EDMS.Components;
using DPWH.EDMS.Web.Client.Shared.Services.Navigation;
using DPWH.EDMS.Client.Shared.Models;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;

namespace DPWH.EDMS.Web.Client.Shared.Nav;

public class NavMenuBase: RxBaseComponent
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
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

    protected string DisplayName = "Juan Doe";
    protected string RoleTitle = "Super Admin";

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

        RxSubscriptions.Add(NavRx.IsExpanded.Subscribe(expanded =>
        {
            if (!expanded)
            {
                SelectedLevel1Item = default;
            }
        }));

    }

    protected async Task BeginSignOut()
    {
        if (AuthenticationStateAsync is null)
            return;

        var authState = await AuthenticationStateAsync;
        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var logoutUrl = user.FindFirst("bff:logout_url")?.Value;
            NavManager.NavigateTo(logoutUrl!,true);
        }
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
        if (!DrawerRef.Expanded)
        {
            NavRx.IsExpanded.OnNext(true);
            DrawerRef.ToggleAsync();
        }

        SelectedLevel1Item = item;
        StateHasChanged();
    }

    protected void OnToggleLevel1Item(MenuModel item)
    {
        if( item.Children != null && item.Children.Count() > 0)
        {
            ToggleLevel1Item(item);
        }
        else
        {
            NavManager.NavigateTo(item.Url ?? "");
        }
    }

}
