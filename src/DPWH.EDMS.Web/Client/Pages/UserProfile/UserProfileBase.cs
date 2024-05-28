using DPWH.EDMS.Components;
using DPWH.EDMS.Client.Shared.Models;
using Telerik.FontIcons;

namespace DPWH.EDMS.Web.Client.Pages.UserProfile;

public class UserProfileBase : RxBaseComponent
{
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = FontIcon.Rows,
            Text = "Profile",
            Url = "/profile"
        });
    }
}
