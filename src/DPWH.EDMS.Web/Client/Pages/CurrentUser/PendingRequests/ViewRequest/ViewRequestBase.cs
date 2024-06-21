using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components.Breadcrumb;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.PendingRequests.ViewRequest;

public class ViewRequestBase : RxBaseComponent
{
    [Parameter] public required string RequestId { get; set; }
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }

    protected RecordRequestModel SelectedRecordRequest { get; set; } = new();

    protected async override Task OnInitializedAsync()
    {
        var recordReq = await RecordRequestsService.GetById(Guid.Parse(RequestId));

        if (recordReq.Success)
        {
            SelectedRecordRequest = recordReq.Data;

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
                    Text = SelectedRecordRequest.ControlNumber.ToString(),
                    Url = NavManager.Uri.ToString(),
                },
            });
        }       
        else
        {
            ToastService.ShowError("Something went wrong on loading record request.");
            NavManager.NavigateTo("/my-pending-request");
        }
    }
}
