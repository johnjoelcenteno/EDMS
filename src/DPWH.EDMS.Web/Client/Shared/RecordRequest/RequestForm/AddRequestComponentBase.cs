using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;

public class AddRequestComponentBase : RxBaseComponent
{
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IRecordRequestSupportingFilesService RecordRequestSupportingFilesService { get; set; }
    protected CreateRecordRequest SelectedItem { get; set; } = new();
    protected FluentValidationValidator? FluentValidationValidator;

    protected string RedirectUri = "/";

    protected void HandleCancel(string uri)
    {
        NavManager.NavigateTo(uri);
    }
    protected async Task HandleSubmit((CreateRecordRequest, UploadRecordRequestDocumentModel, UploadRecordRequestDocumentModel) parameters)
    {
        var (currentDocRequest, validId, suppDoc) = parameters;

        await ExceptionHandlerService.HandleApiException(
        async () =>
        {
            IsLoading = true;
            if (currentDocRequest.Claimant == ClaimantTypes.AuthorizedRepresentative.ToString())
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
                        currentDocRequest.SupportingFileValidId = uploadValidIdRes.Id;
                        CreateResponse uploadSupportingDocRes = new();

                        await ExceptionHandlerService.HandleApiException(async () =>
                        {
                            uploadSupportingDocRes = await RecordRequestSupportingFilesService.Upload(suppDoc.Document, suppDoc.DocumentType, suppDoc.DocumentTypeId);
                        });

                        if (uploadSupportingDocRes.Success)
                        {
                            currentDocRequest.SupportingFileAuthorizationDocumentId = uploadSupportingDocRes.Id;
                            // set values
                            //SelectedItem = currentDocRequest;
                            //SelectedItem.SupportingFileValidId = uploadValidIdRes.Id;
                            //SelectedItem.SupportingFileAuthorizationDocumentId = uploadSupportingDocRes.Id;

                            var createRes = await RequestManagementService.Create(currentDocRequest);

                            if (createRes.Success)
                            {
                                ToastService.ShowSuccess("Successfully created request!");
                                NavManager.NavigateTo(RedirectUri);
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
                var createRes = await RequestManagementService.Create(currentDocRequest);

                if (createRes.Success)
                {
                    ToastService.ShowSuccess("Successfully created request!");
                    NavManager.NavigateTo(RedirectUri);
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
