using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.Grid;
using Telerik.Blazor.Components.Breadcrumb;
using Telerik.Blazor.Components;
using Microsoft.AspNetCore.Components;
using DPWH.EDMS.Api.Contracts;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.Employee.RequestHistory;

public class RequestHistoryBase : RecordRequestGridComponentBase
{
    [Parameter] public required GetUserByIdResult SelectedEmployee { get; set; }

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        EmployeeId = SelectedEmployee.EmployeeId;
        await HandleOnLoadGrid();
        IsLoading = false;
    }

    protected override void OnInitialized()
    {
       // ----
    }

    protected void GoToAddNewRequest()
    {
        HandleGoToAddNewRequest("record-management/request-history/add/" + SelectedEmployee.Id);
    }
    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        HandleSelectedItemOverview(args, "request-management/view-request-form/");
    }
}