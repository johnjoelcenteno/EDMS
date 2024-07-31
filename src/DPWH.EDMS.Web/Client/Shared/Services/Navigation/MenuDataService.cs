using AutoMapper;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.Services.Navigation.Helpers;
using Telerik.Pivot.Core;

namespace DPWH.EDMS.Web.Client.Shared.Services.Navigation;

public class MenuDataService : IMenuDataService
{
    private readonly INavigationService NavigationService;
    private readonly IMapper Mapper;

    public MenuDataService(INavigationService navigationService, IMapper mapper)
    {
        NavigationService = navigationService;
        Mapper = mapper;
    }

    // 3 Level Nav UI Builder
    public async Task<List<MenuModel>> GetNavigationMenuAsync(NavType navType)
    {
        // Create a request object with dynamic parameters
        var dataSourceRequest = new DataSourceRequest { Skip = 0};

        // Fetch the raw menu items data from the data source based on the provided navType
        var menuRes = await NavigationService.QueryByNavType(navType.ToString(), dataSourceRequest);
        var menus = GenericHelper.GetListByDataSource<Api.Contracts.MenuItemModel>(menuRes.Data);

        // Map the raw data to the MenuModel type
        var navMenusAll = Mapper.Map<List<MenuModel>>(menus);

        // Get top-level menus
        var navMenusLevel0 = navMenusAll.Where(x => x.Level == 0).OrderBy(x => x.SortOrder).ToList();

        // Organize the menus into a hierarchical structure
        foreach (var menu in navMenusLevel0)
        {
            // Get first-level children
            var children = navMenusAll
                .Where(x => x.Level == 1 && x.ParentId == menu.Id)
                .OrderBy(x => x.SortOrder)
                .ToList();

            if (children.Any())
            {
                menu.Children = children;

                foreach (var submenu in menu.Children)
                {
                    // Get second-level children (grandchildren)
                    var grandChildren = navMenusAll
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

        return navMenusLevel0;
    }

    public IEnumerable<MenuModel> GetMenuItems() =>
        new List<MenuModel>
        {
        // SUPER ADMIN MENUS
            new ()
            {
                 Text = MenuType.Home.GetDisplayName(),
                Url = "",
                Icon = "home",
                Children = default,
                Level = 0,
                SortOrder = 0,
                AuthorizedRoles = MenuTypeExtensions.HomeRoles

            },
             new MenuModel
        {
            Text = MenuType.RecordsManagement.GetDisplayName(),
            Url = "/records-management",
            Icon = "folder_open",
            Children = default,
            Level = 0,
            SortOrder = 1,
            AuthorizedRoles = MenuTypeExtensions.RecordsManagementRoles
        },
        new MenuModel
        {
            Text = MenuType.RequestManagement.GetDisplayName(),
            Url = "/request-management",
            Icon = "folder_open",
            Children = default,
            Level = 0,
            SortOrder = 2,
            AuthorizedRoles = MenuTypeExtensions.RequestManagementRoles
        },
        new MenuModel
        {
            Text = MenuType.UserManagement.GetDisplayName(),
            Url = "/user-management",
            Icon = "people",
            Children = default,
            Level = 0,
            SortOrder = 3,
            AuthorizedRoles = MenuTypeExtensions.UserManagementRoles
        },
        new MenuModel
        {
            Text = MenuType.ReportsAndAnalytics.GetDisplayName(),
            Url = "/reports-and-analytics",
            Icon = "settings_applications",
            Children = new List<MenuModel>
            {
                new MenuModel
                {
                    Text = MenuType.Reports.GetDisplayName(),
                    Url = "/reports",
                    Icon = "analytics",
                    Children = new List<MenuModel>
                    {
                        new MenuModel
                        {
                            Text = MenuType.ReportsUsers.GetDisplayName(),
                            Url = "/reports/users",
                            Icon = "supervisor_account",
                            Children = default,
                            Level = 2,
                            SortOrder = 0,
                        },
                        new MenuModel
                        {
                            Text = MenuType.ReportsRecordsManagement.GetDisplayName(),
                            Url = "/reports/records-management",
                            Icon = "supervisor_account",
                            Children = default,
                            Level = 2,
                            SortOrder = 1,
                        },
                        new MenuModel
                        {
                            Text = MenuType.ReportsRequestManagement.GetDisplayName(),
                            Url = "/reports/request-management",
                            Icon = "folder_open",
                            Children = default,
                            Level = 2,
                            SortOrder = 2,
                        },
                        new MenuModel
                        {
                            Text = MenuType.ReportsUserManagement.GetDisplayName(),
                            Url = "/reports/user-management",
                            Icon = "manage_accounts",
                            Children = default,
                            Level = 2,
                            SortOrder = 3,
                        },
                         new MenuModel
                        {
                            Text = MenuType.ReportsSystem.GetDisplayName(),
                            Url = "/reports/system",
                            Icon = "settings_applications",
                            Children = default,
                            Level = 2,
                            SortOrder = 4,
                        }
                    },
                    Level = 1,
                    SortOrder = 0,
                },
                new MenuModel
                {
                    Text = MenuType.AuditTrail.GetDisplayName(),
                    Url = "/audit-trail",
                    Icon = "query_stats",
                    Children = new List<MenuModel>
                    {
                        new MenuModel
                        {
                            Text = MenuType.AuditTrailUserActivity.GetDisplayName(),
                            Url = "/audit-trail/user-activity",
                            Icon = "settings_accessibility",
                            Children = default,
                            Level = 2,
                            SortOrder = 0,
                        },
                        new MenuModel
                        {
                            Text = MenuType.AuditTrailRecordsManagement.GetDisplayName(),
                            Url = "/audit-trail/records-management",
                            Icon = "supervisor_account",
                            Children = default,
                            Level = 2,
                            SortOrder = 1,
                        },
                        new MenuModel
                        {
                            Text = MenuType.AuditTrailRequestManagement.GetDisplayName(),
                            Url = "/audit-trail/request-management",
                            Icon = "folder_open",
                            Children = default,
                            Level = 2,
                            SortOrder = 2,
                        },
                        new MenuModel
                        {
                            Text = MenuType.AuditTrailUserManagement.GetDisplayName(),
                            Url = "/audit-trail/user-management",
                            Icon = "manage_accounts",
                            Children = default,
                            Level = 2,
                            SortOrder = 3,
                        },
                         new MenuModel
                        {
                            Text = MenuType.AuditTrailDataLibrary.GetDisplayName(),
                            Url = "/audit-trail/data-library",
                            Icon = "description",
                            Children = default,
                            Level = 2,
                            SortOrder = 4,
                        }
                    },
                    Level = 1,
                    SortOrder = 1,
                }
            },
            Level = 0,
            SortOrder = 4,
            AuthorizedRoles = MenuTypeExtensions.ReportsAndAnalyticsRoles
        },
        new MenuModel
        {
            Text = MenuType.DataLibrary.GetDisplayName(),
            Url = "/data-library",
            Icon = "description",
            Children = default,
            Level = 0,
            SortOrder = 5,
            AuthorizedRoles = MenuTypeExtensions.DataLibraryRoles
        }
    };


    public IEnumerable<MenuModel> GetMenuItems2() =>
        new List<MenuModel>
        {
            new MenuModel
        {
            Text = MenuType.MyRecords.GetDisplayName(),
            Url = "/my-records",
            Icon = "description",
            Children = default,
            Level = 0,
            SortOrder = 2,
            AuthorizedRoles = MenuTypeExtensions.MyRecordsRoles
        },
        new MenuModel
        {
            Text = MenuType.MyRequests.GetDisplayName(),
            Url = "/my-requests",
            Icon = "description",
            Children = default,
            Level = 0,
            SortOrder = 1,
            AuthorizedRoles = MenuTypeExtensions.MyRequestsRoles
        }
        };

    public IEnumerable<MenuModel> GetSettingsItems() =>
        new List<MenuModel>
        {
            new MenuModel
        {
            Text = MenuType.Profile.GetDisplayName(),
            Url = "/profile",
            Icon = "person",
            Children = default,
            Level = 0,
            SortOrder = 0,
            AuthorizedRoles = MenuTypeExtensions.ProfileRoles
        },
        new MenuModel
        {
            Text = MenuType.Settings.GetDisplayName(),
            Url = "/settings",
            Icon = "settings",
            Children = default,
            Level = 0,
            SortOrder = 1,
            AuthorizedRoles = MenuTypeExtensions.SettingsRoles
        },
        new MenuModel
        {
            Text = MenuType.Support.GetDisplayName(),
            Url = "/support",
            Icon = "help",
            Children = default,
            Level = 0,
            SortOrder = 2,
            AuthorizedRoles = MenuTypeExtensions.SupportRoles
        }
        };
}


/** DON'T DELETE, FOR REFERENCE: **/

//new ()
//{
//    Text = "Test Parent",
//    Url = "",
//    Icon = "home",
//    Children = new List<MenuModel>{
//        new ()
//        {
//            Text = "Test Child",
//            Url = "/test-child-link",
//            Icon = "people",
//            Children = default,
//            Level = 1,
//            SortOrder = 0,
//            AuthorizedRoles = new List<string>{}
//        },
//        new ()
//        {
//            Text = "Test Child 2",
//            Url = "",
//            Icon = "people",
//            Children = new List<MenuModel>{
//                 new ()
//                {
//                    Text = "Test Grand Child 1",
//                    Url = "/test-grandchild-link",
//                    Icon = "people",
//                    Children = default,
//                    Level = 2,
//                    SortOrder = 0,
//                    AuthorizedRoles = new List<string>{}
//                },
//            },
//            Level = 1,
//            SortOrder = 0,
//            AuthorizedRoles = new List<string>{}
//        },
//    },
//    Level = 0,
//    SortOrder = 0,
//    AuthorizedRoles = new List<string>{}
//},

