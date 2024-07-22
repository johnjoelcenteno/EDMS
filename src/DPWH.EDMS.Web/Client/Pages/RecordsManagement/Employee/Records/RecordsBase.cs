using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Web.Client.Shared.Records.Grid;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.Employee.Records;

public class RecordsBase : RecordsGridComponentBase
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

    //protected void GoToAddNewRequest()
    //{
    //    HandleGoToAddNewRequest("record-management/request-history/add/" + SelectedEmployee.Id);
    //}

    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        //HandleSelectedItemOverview(args, "request-management/view-request-form/");
    }
}
