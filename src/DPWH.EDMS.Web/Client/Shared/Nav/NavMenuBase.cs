using DPWH.EDMS.Client.Shared.Configurations;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Web.Client.Shared.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Telerik.Blazor.Components;

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
    protected bool XSmall { get; set; }
    //protected bool Small { get; set; }
    //protected bool Medium { get; set; }
    //protected bool Large { get; set; }
    protected List<IDisposable> RxSubscriptions { get; set; } = new();

    protected string DisplayName = "---";
    protected string Role = string.Empty;

    protected MenuModel? SelectedLevel1Item = null;

    //protected readonly string LoginUrl = "bff/login";

    protected override async Task OnInitializedAsync()
    {
        await SetMenu();

        RxSubscriptions.Add(NavRx.IsExpanded.Subscribe(expanded =>
        {
            if (!expanded)
            {
                SelectedLevel1Item = default;
            }

            #pragma warning disable BL0005 // Component parameter should not be set outside of its component.
            DrawerRef.Expanded = expanded;
            #pragma warning restore BL0005 // Component parameter should not be set outside of its component.
        }));

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
            var roleValue = user.Claims.FirstOrDefault(c => c.Type == "role")!.Value;
            DisplayName = !string.IsNullOrEmpty(user.Identity.Name) ? GenericHelper.CapitalizeFirstLetter(user.Identity.Name) : "---";
            Role = GetRoleLabel(roleValue);
            NavMenus = MenuDataService.GetMenuItems().Where(m => m.AuthorizedRoles.Any(r => r == roleValue)).ToList();
            NavMenus2 = MenuDataService.GetMenuItems2().Where(m => m.AuthorizedRoles.Any(r => r == roleValue)).ToList();
            NavSettings = MenuDataService.GetSettingsItems().Where(m => m.AuthorizedRoles.Any(r => r == roleValue)).ToList();            
        }        
    }

    private string GetRoleLabel(string roleValue)
    {
        return ApplicationRoles.GetDisplayRoleName(roleValue, "Unknown Role");
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

    protected void OnXsSidebarCollapse()
    {
        IsNavMenuCollapsed = !IsNavMenuCollapsed;
        StateHasChanged();
    }
}
