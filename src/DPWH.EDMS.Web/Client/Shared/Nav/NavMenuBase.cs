﻿using AutoMapper;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;
using DPWH.EDMS.Client.Shared.Configurations;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Shared.Enums;
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
    [Inject] public required INavigationService NavigationService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }

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
           
            var role = roles.FirstOrDefault(role => !string.IsNullOrEmpty(role.Value) && role.Value.Contains(ApplicationRoles.RolePrefix))?.Value ?? string.Empty;

            var firstnameValue = user.Claims.FirstOrDefault(x => x.Type == "firstname")?.Value;
            var lastnameValue = user.Claims.FirstOrDefault(x => x.Type == "lastname")?.Value;
            var office = user.Claims.FirstOrDefault(x => x.Type == "office")?.Value;

            DisplayName = (!string.IsNullOrEmpty(firstnameValue) && !string.IsNullOrEmpty(lastnameValue))
                ? GenericHelper.CapitalizeFirstLetter($"{firstnameValue} {lastnameValue}")
                : "---";

            Office = !string.IsNullOrEmpty(office) ? GetOfficeName(office) : "---";
            Role = GetRoleLabel(role);

            // TEST: Get current user menus
            var currentUserMenusRes = await NavigationService
                .QueryByNavType(NavType.CurrentUserMenu.ToString(), new DataSourceRequest() { Skip = 0 });
            var currentUserMenus = GenericHelper.GetListByDataSource<Api.Contracts.MenuItemModel>(currentUserMenusRes.Data);
            var navMenus2All = Mapper.Map<List<MenuModel>>(currentUserMenus);
            NavMenus2 = navMenus2All.Where(x => x.Level == 0).ToList();

            foreach (var menu in NavMenus2)
            {
                var children = navMenus2All
                    .Where(x => x.Level == 1 && x.ParentId == menu.Id)
                    .OrderBy(x => x.SortOrder)
                    .ToList();
                if (children.Any())
                {
                    menu.Children = children;

                    foreach (var submenu in menu.Children)
                    {
                        var grandChildren = navMenus2All
                        .Where(x => x.Level == 2 && x.ParentId == submenu.Id)
                        .OrderBy(x => x.SortOrder)
                        .ToList();

                        if (grandChildren.Any())
                        {
                            submenu.Children = grandChildren;
                        }
                    }
                }
            }

            //foreach (var menu in NavMenus2)
            //{
            //    var children = navMenus2All
            //        .Where(x => x.Level == 1 && x.ParentId == menu.Id)
            //        .OrderBy(x => x.SortOrder)
            //        .ToList();
            //    if (children.Any())
            //    {
            //        menu.Children = children;
            //        var grandChildren = navMenus2All
            //            .Where(x => x.Level == 2 && x.ParentId == menu.Id)
            //            .OrderBy(x => x.SortOrder)
            //            .ToList();

            //        if (grandChildren.Any())
            //        {
            //            // todo
            //        }
            //    }
            //}

            // TEST: Get settings menus
            var settingsMenusRes = await NavigationService.QueryByNavType(NavType.Settings.ToString(), new DataSourceRequest() { Skip = 0 });
            var settingsMenus = GenericHelper.GetListByDataSource<Api.Contracts.MenuItemModel>(settingsMenusRes.Data).OrderBy(menu => menu.SortOrder).ToList(); 
            NavSettings = Mapper.Map<List<MenuModel>>(settingsMenus);


            NavMenus = MenuDataService.GetMenuItems().Where(m => m.AuthorizedRoles.Any(r => r == role)).ToList();

            //NavMenus = MenuDataService.GetMenuItems().Where(m => m.AuthorizedRoles.Any(r => r == role)).ToList();
            //NavMenus2 = MenuDataService.GetMenuItems2().Where(m => m.AuthorizedRoles.Any(r => r == role)).ToList();
            //NavSettings = MenuDataService.GetSettingsItems().Where(m => m.AuthorizedRoles.Any(r => r == role)).ToList();
        }
    }

    protected string GetOfficeName(string officeCode)
    {
        return officeCode switch
        {
            "RMD" => "Records Management Division",
            "HRMD" => "Human Resource Management Division",
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
