using DPWH.EDMS.Client.Shared.Models;

namespace DPWH.EDMS.Web.Client.Shared.Services.Navigation;

public class MenuDataService : IMenuDataService
{
    public IEnumerable<MenuModel> GetMenuItems() =>
    new List<MenuModel>
        {
            new ()
            {
                Text = "Dashboard",
                Url = "",
                Icon = "dashboard",
                Children = default,
                Level = 0,
                SortOrder = 0,
                AuthorizedRoles = new List<string>{}
            },
             new ()
            {
                Text = "Test 1",
                Url = "/test-1",
                Icon = "home",
                Children = default,
                Level = 0,
                SortOrder = 0,
                AuthorizedRoles = new List<string>{}
            },
            new ()
            {
                Text = "Test Parent",
                Url = "",
                Icon = "home",
                Children = new List<MenuModel>{
                    new ()
                    {
                        Text = "Test Child",
                        Url = "/test-child-link",
                        Icon = "people",
                        Children = default,
                        Level = 1,
                        SortOrder = 0,
                        AuthorizedRoles = new List<string>{}
                    },
                    new ()
                    {
                        Text = "Test Child 2",
                        Url = "",
                        Icon = "people",
                        Children = new List<MenuModel>{
                             new ()
                            {
                                Text = "Test Grand Child 1",
                                Url = "/test-grandchild-link",
                                Icon = "people",
                                Children = default,
                                Level = 2,
                                SortOrder = 0,
                                AuthorizedRoles = new List<string>{}
                            },
                        },
                        Level = 1,
                        SortOrder = 0,
                        AuthorizedRoles = new List<string>{}
                    },
                },
                Level = 0,
                SortOrder = 0,
                AuthorizedRoles = new List<string>{}
            },
        };
}
