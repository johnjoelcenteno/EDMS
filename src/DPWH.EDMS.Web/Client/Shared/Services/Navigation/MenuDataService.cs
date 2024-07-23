using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Web.Client.Shared.Services.Navigation.Helpers;

namespace DPWH.EDMS.Web.Client.Shared.Services.Navigation;

public class MenuDataService : IMenuDataService
{
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
                            Text = MenuType.Users.GetDisplayName(),
                            Url = "/users",
                            Icon = "supervisor_account",
                            Children = default,
                            Level = 2,
                            SortOrder = 0,
                        },
                        new MenuModel
                        {
                            Text = MenuType.RecordsManagementReport.GetDisplayName(),
                            Url = "/records-management",
                            Icon = "supervisor_account",
                            Children = default,
                            Level = 2,
                            SortOrder = 0,
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
                    Children = default,
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

