using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Shared.Enums;

namespace DPWH.EDMS.Web.Client.Shared.Services.Navigation;
public interface IMenuDataService
{
    IEnumerable<MenuModel> GetMenuItems();
    IEnumerable<MenuModel> GetMenuItems2();
    IEnumerable<MenuModel> GetSettingsItems();
    Task<List<MenuModel>> GetNavigationMenuAsync(NavType navType);
}
