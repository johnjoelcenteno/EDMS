﻿using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.IDP.Core.Constants;

namespace DPWH.EDMS.Web.Client.Shared.Services.Navigation;

public class MenuDataService : IMenuDataService
{
    public IEnumerable<MenuModel> GetMenuItems() =>
    new List<MenuModel>
        {
        // SUPER ADMIN MENUS
            new ()
            {
                Text = "Home",
                Url = "",
                Icon = "home",
                Children = default,
                Level = 0,
                SortOrder = 0,
                AuthorizedRoles = new List<string>{ ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin }
            },
             new ()
            {
                Text = "Request Management",
                Url = "/request-management",
                Icon = "folder_open",
                Children = new List<MenuModel>()
                {
                    new ()
                    {
                        Text = "Pending Request",
                        Url = "/my-pending-request",
                        Icon = "description",
                        Children = default,
                        Level = 0,
                        SortOrder = 0,
                        AuthorizedRoles = new List<string>{ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin, ApplicationRoles.EndUser }
                    },
                    new ()
                    {
                        Text = "Request Management",
                        Url = "/request-management",
                        Icon = "description",
                        Children = default,
                        Level = 0,
                        SortOrder = 1,
                        AuthorizedRoles = new List<string>{ ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin }
                    },
                },
                Level = 0,
                SortOrder = 1,
                AuthorizedRoles = new List<string>{ ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin }
            },

             new ()
            {
                Text = "Records Management",
                Url = "/records-management",
                Icon = "folder_open",
                Children = default,
                Level = 0,
                SortOrder = 2,
                AuthorizedRoles = new List<string>{ ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin }
            },
             new ()
            {
                Text = "Data Library",
                Url = "/data-library",
                Icon = "settings_applications",
                Children = default,
                Level = 0,
                SortOrder = 3,
                AuthorizedRoles = new List<string>{ ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin }
            },
              new ()
            {
                Text = "User Management",
                Url = "/user-management",
                Icon = "people",
                Children = default,
                Level = 0,
                SortOrder = 3,
                AuthorizedRoles = new List<string>{ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin }
            },
        };

    public IEnumerable<MenuModel> GetMenuItems2() =>
        new List<MenuModel>
        {
            new ()
            {
                Text = "My Pending Request",
                Url = "/my-pending-request",
                Icon = "description",
                Children = default,
                Level = 0,
                SortOrder = 0,
                AuthorizedRoles = new List<string>{ApplicationRoles.EndUser }
            },
            new ()
            {
                Text = "My Request History",
                Url = "/my-request-history",
                Icon = "description",
                Children = default,
                Level = 0,
                SortOrder = 1,
                AuthorizedRoles = new List<string>{ ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin, ApplicationRoles.EndUser }
            },
            new ()
            {
                Text = "My Records",
                Url = "/my-records",
                Icon = "description",
                Children = default,
                Level = 0,
                SortOrder = 2,
                AuthorizedRoles = new List<string>{ ApplicationRoles.SuperAdmin, ApplicationRoles.SystemAdmin, ApplicationRoles.EndUser }
            },
        };

    public IEnumerable<MenuModel> GetSettingsItems() =>
        new List<MenuModel>
        {
            new ()
            {
                Text = "Profile",
                Url = "/profile",
                Icon = "person",
                Children = default,
                Level = 0,
                SortOrder = 0,
                AuthorizedRoles = new List<string>{ApplicationRoles.SuperAdmin, ApplicationRoles.EndUser, ApplicationRoles.SystemAdmin }
            },
             new ()
            {
                Text = "Settings",
                Url = "/settings",
                Icon = "settings",
                Children = default,
                Level = 0,
                SortOrder = 1,
                AuthorizedRoles = new List<string>{ ApplicationRoles.SuperAdmin, ApplicationRoles.EndUser, ApplicationRoles.SystemAdmin }
            },
             new ()
            {
                Text = "Support",
                Url = "/support",
                Icon = "help",
                Children = default,
                Level = 0,
                SortOrder = 2,
                AuthorizedRoles = new List<string>{ ApplicationRoles.SuperAdmin, ApplicationRoles.EndUser, ApplicationRoles.SystemAdmin }
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

