using DPWH.EDMS.Client.Shared.Models;

namespace DPWH.EDMS.Web.Client.Shared.Services.Navigation;

public class MenuDataService : IMenuDataService
{
    public IEnumerable<MenuModel> GetMenuItems() =>
    new List<MenuModel>
        {
            new ()
            {
                Text = "Home",
                Url = "",
                Icon = "home",
                Children = default,
                Level = 0,
                SortOrder = 0,
                AuthorizedRoles = new List<string>{}
            },
             new ()
            {
                Text = "User Profile",
                Url = "/user-profile",
                Icon = "person",
                Children = default,
                Level = 0,
                SortOrder = 1,
                AuthorizedRoles = new List<string>{}
            },
             new ()
            {
                Text = "Records Management",
                Url = "/records-management",
                Icon = "folder_open",
                Children = default,
                Level = 0,
                SortOrder = 2,
                AuthorizedRoles = new List<string>{}
            },
             new ()
            {
                Text = "Request Management",
                Url = "/request-management",
                Icon = "description",
                Children = default,
                Level = 0,
                SortOrder = 3,
                AuthorizedRoles = new List<string>{}
            },
             new ()
            {
                Text = "User Management",
                Url = "/user-management",
                Icon = "group",
                Children = default,
                Level = 0,
                SortOrder = 4,
                AuthorizedRoles = new List<string>{}
            },
             new ()
            {
                Text = "Data Library",
                Url = "/data-library",
                Icon = "library_books",
                Children = default,
                Level = 0,
                SortOrder = 5,
                AuthorizedRoles = new List<string>{}
            },
             new ()
            {
                Text = "Reports and Analytics",
                Url = "/reports-and-analytics",
                Icon = "area_chart",
                Children = default,
                Level = 0,
                SortOrder = 6,
                AuthorizedRoles = new List<string>{}
            },
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
        };
}
