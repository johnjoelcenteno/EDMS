using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Shared.Enums;
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
    [Inject] public required IRecordRequestSupportingFilesService RecordRequestSupportingFilesService { get; set; }
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

    protected async Task HandleSubmit((CreateRecordRequest, UploadSupportingFileRequestModel, UploadSupportingFileRequestModel) parameters)
    {
        var (currentDocRequest, validId, suppDoc) = parameters;

        await ExceptionHandlerService.HandleApiException(
        async () =>
        {
            IsLoading = true;
            if(currentDocRequest.Claimant == ClaimantTypes.AuthorizedRepresentative.ToString())
            {
                if ((currentDocRequest != null && validId != null))
                {
                    CreateResponse uploadValidIdRes = new();

                    await ExceptionHandlerService.HandleApiException(async () =>
                    {
                        uploadValidIdRes = await RecordRequestSupportingFilesService.Upload(validId.document, validId.documentType, validId.documentTypeId);
                    });

                    if (uploadValidIdRes.Success)
                    {

                        CreateResponse uploadSupportingDocRes = new();
                        currentDocRequest.ValidId = uploadValidIdRes.Id;

                        await ExceptionHandlerService.HandleApiException(async () =>
                        {
                            uploadSupportingDocRes = await RecordRequestSupportingFilesService.Upload(suppDoc.document, suppDoc.documentType, suppDoc.documentTypeId);
                        });

                        if (uploadSupportingDocRes.Success)
                        {
                            // set values
                            //SelectedItem = currentDocRequest;
                            currentDocRequest.SupportingDocument = uploadSupportingDocRes.Id;

                            var createRes = await RecordRequestsService.CreateRecordRequest(currentDocRequest);

                            if (createRes.Success)
                            {
                                ToastService.ShowSuccess("Successfully created request!");
                                NavManager.NavigateTo("/my-pending-request");
                            }
                            else
                            {
                                ToastService.ShowError("Something went wrong on creating request.");
                            }
                        }
                        else
                        {
                            ToastService.ShowError("Something went wrong uploading supporting document.");
                        }
                    }
                    else
                    {
                        ToastService.ShowError("Something went wrong uploading valid id.");
                    }
                }
            }            
            else
            {
                var createRes = await RecordRequestsService.CreateRecordRequest(currentDocRequest);

                if (createRes.Success)
                {
                    ToastService.ShowSuccess("Successfully created request!");
                    NavManager.NavigateTo("/my-pending-request");
                }
                else
                {
                    ToastService.ShowError("Something went wrong on creating request.");
                }
            }
            IsLoading = false;
        });
    }

}
