using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests.AddRequest;

public class AddRequestBase : AddRequestComponentBase
{
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Add New Request",
            Url = NavManager.Uri.ToString(),
        });
    }

    protected void OnCancel()
    {
        NavManager.NavigateTo("/my-requests");
    }

}
