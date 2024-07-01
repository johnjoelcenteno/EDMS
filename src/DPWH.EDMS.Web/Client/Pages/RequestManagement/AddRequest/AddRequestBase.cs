using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.AddRequest;

public class AddRequestBase : RxBaseComponent
{
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IRecordRequestSupportingFilesService RecordRequestSupportingFilesService { get; set; }
    protected CreateRecordRequest SelectedItem { get; set; } = new();
    protected FluentValidationValidator? FluentValidationValidator;

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
        NavManager.NavigateTo("/request-management");
    }

    protected async Task HandleSubmit((CreateRecordRequest, UploadRecordRequestDocumentModel, UploadRecordRequestDocumentModel) parameters)
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
                        uploadValidIdRes = await RecordRequestSupportingFilesService.Upload(validId.Document, validId.DocumentType, validId.DocumentTypeId);
                    });

                    if (uploadValidIdRes.Success)
                    {

                        CreateResponse uploadSupportingDocRes = new();

                        await ExceptionHandlerService.HandleApiException(async () =>
                        {
                            uploadSupportingDocRes = await RecordRequestSupportingFilesService.Upload(suppDoc.Document, suppDoc.DocumentType, suppDoc.DocumentTypeId);
                        });

                        if (uploadSupportingDocRes.Success)
                        {
                            // set values
                            SelectedItem = currentDocRequest;
                            SelectedItem.ValidId = uploadValidIdRes.Id;

                            var createRes = await RecordRequestsService.CreateRecordRequest(currentDocRequest);

                            if (createRes.Success)
                            {
                                ToastService.ShowSuccess("Successfully created request!");
                                NavManager.NavigateTo("/request-management");
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
                    NavManager.NavigateTo("/request-management");
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
