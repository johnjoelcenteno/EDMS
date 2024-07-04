using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;

public class RequestDetailsOverviewBase : RxBaseComponent
{
    [Parameter] public required string RequestId { get; set; }
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }

    protected RecordRequestModel SelectedRecordRequest { get; set; } = new();

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
        await ExceptionHandlerService.HandleApiException( async () => {
            var recordReq = await RequestManagementService.GetById(Guid.Parse(RequestId));

            if (recordReq.Success)
            {
                if (onLoadCb != null)
                {
                    onLoadCb.Invoke(recordReq.Data);
                }
            }
            else
            {
                ToastService.ShowError("Something went wrong on loading record request.");
                NavManager.NavigateTo("/my-requests");
            }
        });        
    }

    protected string GetValidIdTextDisplay()
    {
        return
            SelectedRecordRequest.AuthorizedRepresentative != null &&
            GenericHelper.IsGuidHasValue(SelectedRecordRequest.AuthorizedRepresentative.ValidId) &&
            !string.IsNullOrEmpty(SelectedRecordRequest.AuthorizedRepresentative.ValidIdUri)
                ? SelectedRecordRequest.AuthorizedRepresentative.ValidIdName
                : "No ID attached.";

    }

    protected string GetSupportingDocTextDisplay()
    {
        return
            SelectedRecordRequest.AuthorizedRepresentative != null &&
            GenericHelper.IsGuidHasValue(SelectedRecordRequest.AuthorizedRepresentative.AuthorizationDocumentId) &&
            !string.IsNullOrEmpty(SelectedRecordRequest.AuthorizedRepresentative.AuthorizationDocumentUri)
                ? SelectedRecordRequest.AuthorizedRepresentative.AuthorizationDocumentName
                : "No Document attached.";

    }
}
