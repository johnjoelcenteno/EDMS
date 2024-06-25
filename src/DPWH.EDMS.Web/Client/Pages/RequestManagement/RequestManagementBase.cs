using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.IDP.Core.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement;

public class RequestManagementBase : GridBase<RecordRequestModel>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }

    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Request Management",
            Url = NavManager.Uri.ToString(),
        });
    }

    protected async override Task OnInitializedAsync()
    {
        ServiceCb = RecordRequestsService.Query;
        await LoadData();
    }

    protected void GoToAddNewRequest()
    {
        NavManager.NavigateTo("request-management/add");
    }
    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        IsLoading = true;

        var selectedItem = args.Item as RecordRequestModel;

        if (selectedItem != null)
        {
            NavManager.NavigateTo("request-management/view/" + selectedItem.Id.ToString());
        }

        IsLoading = false;
    }
}
