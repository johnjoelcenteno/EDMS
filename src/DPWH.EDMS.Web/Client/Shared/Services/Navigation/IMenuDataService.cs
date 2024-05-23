using DPWH.EDMS.Client.Shared.Models;

namespace DPWH.EDMS.Web.Client.Shared.Services.Navigation;
public interface IMenuDataService
{
    IEnumerable<MenuModel> GetMenuItems();
    IEnumerable<MenuModel> GetMenuItems2();
    IEnumerable<MenuModel> GetSettingsItems();
}
