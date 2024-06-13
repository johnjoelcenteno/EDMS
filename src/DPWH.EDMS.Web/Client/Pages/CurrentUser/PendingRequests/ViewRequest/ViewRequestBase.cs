using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components.Breadcrumb;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.PendingRequests.ViewRequest;

public class ViewRequestBase : RxBaseComponent
{
    [Parameter] public string ControlNumber { get; set; } = "CA00001_TEST";
    protected override void OnInitialized()
    {
        BreadcrumbItems.AddRange(new List<BreadcrumbModel>
        {
            new BreadcrumbModel
            {
                Icon = "menu",
                Text = "My Pending Request",
                Url = "/my-pending-request",
            },
            new BreadcrumbModel
            {
                Icon = "create_new_folder",
                Text = GenericHelper.GetDisplayValue(ControlNumber),
                Url = NavManager.Uri.ToString(),
            },
        });
    }
}
