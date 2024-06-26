using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests;

public class MyRequestsBase : RxBaseComponent
{
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Requests",
            Url = NavManager.Uri.ToString(),
        });
    }
}
