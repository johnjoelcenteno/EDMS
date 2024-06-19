using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.PendingRequests.AddRequest;

public class AddRequestBase : RxBaseComponent
{
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    protected CreateRecordRequest SelectedItem { get; set; } = new();
    protected FluentValidationValidator? FluentValidationValidator;
    protected bool IsLoading = false;

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
        NavManager.NavigateTo("/my-pending-request");
    }

    protected async Task HandleSubmit(CreateRecordRequest currentDocRequest)
    {
        await ExceptionHandlerService.HandleApiException(
        async () =>
        {
            IsLoading = true;
            if (currentDocRequest != null)
            {
                SelectedItem = currentDocRequest;
                var createRes = await RecordRequestsService.CreateRecordRequest(currentDocRequest);

                if (createRes.Success)
                {
                    ToastService.ShowSuccess("Successfully created request!");
                    IsLoading = false;
                    NavManager.NavigateTo("/my-pending-request");
                }
            }
        });
    }

}
