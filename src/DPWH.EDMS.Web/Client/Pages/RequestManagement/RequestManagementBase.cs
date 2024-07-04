using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.Grid;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement;

public class RequestManagementBase : RecordRequestGridComponentBase
{
    protected async override Task OnInitializedAsync()
    {
        await HandleOnLoadGrid();
    }

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Request Management",
            Url = NavManager.Uri.ToString(),
        });
    }

    protected void GoToAddNewRequest()
    {
        HandleGoToAddNewRequest("request-management/add");
    }
    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        HandleSelectedItemOverview(args, "request-management/view-request-form");
    }
}
