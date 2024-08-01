using AutoMapper;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;
using DPWH.EDMS.Client.Shared.Configurations;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.Services.Navigation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Shared.Nav;

public class NavMenuBase : RxBaseComponent
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    //[Inject] public required AuthRxService AuthRxService { get; set; }
    [Inject] public required ConfigManager ConfigManager { get; set; }
    [Inject] public required IMenuDataService MenuDataService { get; set; }
    [Inject] public required INavigationService NavigationService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }
    [Inject] public required NavRx NavRx { get; set; }

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
    protected string Office = string.Empty;

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

        var authState = await AuthenticationStateAsync;
        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var roles = user.Claims.Where(c => c.Type == "role")!.ToList();

            var role = roles.FirstOrDefault(role => !string.IsNullOrEmpty(role.Value) && role.Value.Contains(ApplicationRoles.RolePrefix))?.Value ?? ClaimsPrincipalExtensions.GetRole(user);

            var firstnameValue = ClaimsPrincipalExtensions.GetFirstName(user);
            var lastnameValue = ClaimsPrincipalExtensions.GetLastName(user);
            var office = ClaimsPrincipalExtensions.GetOffice(user);

            DisplayName = (!string.IsNullOrEmpty(firstnameValue) && !string.IsNullOrEmpty(lastnameValue))
                ? GenericHelper.CapitalizeFirstLetter($"{firstnameValue} {lastnameValue}")
                : "---";

            Office = !string.IsNullOrEmpty(office) ? GetOfficeName(office) : "---";
            Role = GetRoleLabel(role);

            NavMenus = MenuDataService.GetMenuItems().Where(m => m.AuthorizedRoles.Any(r => r == role)).ToList();
            //NavMenus2 = MenuDataService.GetMenuItems2().Where(m => m.AuthorizedRoles.Any(r => r == role)).ToList();
            NavSettings = MenuDataService.GetSettingsItems().Where(m => m.AuthorizedRoles.Any(r => r == role)).ToList();

            NavMenus2 = await MenuDataService.GetNavigationMenuAsync(NavType.CurrentUserMenu);

            // DO NOT DELETE: FOR REFERENCE
            // TEST: Get current user menus
            //var currentUserMenusRes = await NavigationService
            //    .QueryByNavType(NavType.CurrentUserMenu.ToString(), new DataSourceRequest() { Skip = 0 });
            //var currentUserMenus = GenericHelper.GetListByDataSource<Api.Contracts.MenuItemModel>(currentUserMenusRes.Data);
            //var navMenus2All = Mapper.Map<List<MenuModel>>(currentUserMenus);
            //NavMenus2 = navMenus2All.Where(x => x.Level == 0).OrderBy(x => x.SortOrder).ToList();

            //foreach (var menu in NavMenus2)
            //{
            //    var children = navMenus2All
            //        .Where(x => x.Level == 1 && x.ParentId == menu.Id)
            //        .OrderBy(x => x.SortOrder)
            //        .ToList();
            //    if (children.Any())
            //    {
            //        menu.Children = children;

            //        foreach (var submenu in menu.Children)
            //        {
            //            var grandChildren = navMenus2All
            //            .Where(x => x.Level == 2 && x.ParentId == submenu.Id)
            //            .OrderBy(x => x.SortOrder)
            //            .ToList();

            //            if (grandChildren.Any())
            //            {
            //                submenu.Children = grandChildren;
            //            }
            //        }
            //    }
            //}
        }
    }

    protected string GetOfficeName(string officeCode)
    {
        return officeCode switch
        {
            nameof(Offices.RMD) => "Records Management Division",
            nameof(Offices.HRMD) => "Human Resource Management Division",
            _ => string.Empty
        };
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
        if (item.Children != null && item.Children.Count() > 0)
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

    protected bool IsEdmsLogin() => NavManager.Uri.EndsWith("/edms/login", StringComparison.OrdinalIgnoreCase);
}
