using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.Employee.RequestHistory.AddRequest;

public class AddRequestBase : AddRequestComponentBase
{
    [Parameter] public required string UserId { get; set; }
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Add New Request",
            Url = NavManager.Uri.ToString(),
        });

        RedirectUri = "/records-management/" + UserId;
    }

    protected void OnCancel()
    {
        HandleCancel(RedirectUri);
    }
}
