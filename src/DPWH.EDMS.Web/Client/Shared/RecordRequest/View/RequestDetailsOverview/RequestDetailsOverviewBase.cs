using Blazored.Toast.Services;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Components;
using Microsoft.AspNetCore.Components;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Shared.Enums;

namespace DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;

public class RequestDetailsOverviewBase : RxBaseComponent
{
    [Parameter] public required string RequestId { get; set; }
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }

    protected RecordRequestModel SelectedRecordRequest { get; set; } = new();
    protected RecordRequestDocumentModel SelectedValid { get; set; } = new();
    protected RecordRequestDocumentModel SelectedAuthorizedDocument { get; set; } = new();

    protected string CancelReturnUrl = string.Empty;

    protected virtual void OnCancel()
    {
        if (!string.IsNullOrEmpty(CancelReturnUrl))
        {
            NavManager.NavigateTo(CancelReturnUrl);
        }
    }

    protected async Task LoadData(Action<RecordRequestModel> onLoadCb)
    {
        var recordReq = await RecordRequestsService.GetById(Guid.Parse(RequestId));

        if (recordReq.Success)
        {
            if(onLoadCb != null)
            {
                onLoadCb.Invoke(recordReq.Data);
            }
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading record request.");
            NavManager.NavigateTo("/my-requests");
        }
    }
}
