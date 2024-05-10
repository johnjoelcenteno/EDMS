using DPWH.EDMS.Web.Shared.Models;

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
        };
}
