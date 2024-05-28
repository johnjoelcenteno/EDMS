using DPWH.EDMS.Client.Shared.Configurations;
using DPWH.EDMS.Components;
using DPWH.EDMS.Web.Client.Shared.Services.Navigation;
using DPWH.EDMS.Client.Shared.Models;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using DPWH.EDMS.IDP.Core.Constants;
using System.Security.Claims;

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
    protected List<MenuModel> NavMenus2 { get; set; } = new List<MenuModel>();
    protected List<MenuModel> NavSettings { get; set; } = new List<MenuModel>();
    //protected bool XSmall { get; set; }
    //protected bool Small { get; set; }
    //protected bool Medium { get; set; }
    //protected bool Large { get; set; }
    protected List<IDisposable> RxSubscriptions { get; set; } = new();

    protected string DisplayName = "Juan Doe";
    protected string Role = ApplicationRoles.EndUser;

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

            DrawerRef.Expanded = expanded;
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

    protected string GetRoleDisplayText()
    { 

        var displayName = "";
        ApplicationRoles.UserAccessMapping.TryGetValue(Role, out displayName);

        return !string.IsNullOrEmpty(displayName) ? displayName : "Unknown Role";
    }

    private async Task SetMenu()
    {
        if (AuthenticationStateAsync is null)
            return;

        //var menus = MenuDataService.GetMenuItems();

        var authState = await AuthenticationStateAsync;
        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            //DisplayName = user.FullName;
            //GetRoleDisplayText();
            NavMenus = MenuDataService.GetMenuItems().Where(m => m.AuthorizedRoles.Any(r => r == Role)).ToList();
            NavMenus2 = MenuDataService.GetMenuItems2().Where(m => m.AuthorizedRoles.Any(r => r == Role)).ToList();
            NavSettings = MenuDataService.GetSettingsItems().Where(m => m.AuthorizedRoles.Any(r => r == Role)).ToList();
        }        
    }

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
