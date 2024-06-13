using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.PendingRequests.RequestForm;

public class RequestFormBase : RxBaseComponent
{
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required IDocumentService DocumentService { get; set; }

    [Parameter] public bool IsEditMode { get; set; } = false;
    [Parameter] public EventCallback<DocumentRequestModel> HandleOnSubmit { get; set; }
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    protected DocumentRequestModel SelectedItem { get; set; } = new();

    protected List<DocumentClaimant> documentClaimants = Enum.GetValues(typeof(DocumentClaimant)).Cast<DocumentClaimant>().ToList();
    protected List<string> allRecords = new List<string> { "Record 1", "Record 2", "Record 3" };
    protected List<string> validIdTypes = new List<string> { "ID Type 1", "ID Type 2", "ID Type 3" };
    protected List<string> supportingDocumentTypes = new List<string> { "Document Type 1", "Document Type 2", "Document Type 3" };

    protected FluentValidationValidator? FluentValidationValidator;

    protected TelerikUpload ValidIdUploadRef { get; set; }
    protected TelerikUpload SupportedDocumentUploadRef { get; set; }
    public int MinFileSize { get; set; } = 1024;
    public int MaxFileSize { get; set; } = 4 * 1024 * 1024;
    public List<string> AllowedExtensions { get; set; } = new List<string>() { ".docx", ".pdf" };
    Dictionary<string, bool> FilesValidationInfo { get; set; } = new Dictionary<string, bool>();

    protected override void OnInitialized()
    {

    }

    protected async Task HandleOnSubmitCallback()
    {
        if (await FluentValidationValidator!.ValidateAsync())
        {
            //var createRes = await Service!.create(SelectedItem);

            //if (createRes.Success)
            //{
            //    ToastService!.ShowSuccess("Successfully created address!");
            //}
            //else
            //{
            //    ToastService!.ShowError($"Something went wrong on creating address! {createRes.Error}");
            //}
            if (HandleOnSubmit.HasDelegate)
            {
                await HandleOnSubmit.InvokeAsync(SelectedItem);
                ToastService!.ShowSuccess("Successfully created request!");
            }

        }
        else
        {
            ToastService!.ShowError("Something went wrong!");
            await HandleOnSubmit.InvokeAsync(null);
        }       
    }
    
    protected async Task HandleOnCancelCallback()
    {
        if (HandleOnCancel.HasDelegate)
        {
            await HandleOnCancel.InvokeAsync();
        }
    }

    protected async void OnSelect(FileSelectEventArgs args)
    {
        foreach (var file in args.Files)
        {
            SelectedItem.IsValidIdAccepted = IsSelectedFileValid(file);

            if (SelectedItem.IsValidIdAccepted)
            {
                //FileParameter item = await DocumentService.GetFileToUpload(args);
            }
        }
    }

    protected void OnRemove(FileSelectEventArgs args)
    {
        foreach (var file in args.Files)
        {
            SelectedItem.IsValidIdAccepted = IsSelectedFileValid(file);
        }
    }
    protected bool IsSelectedFileValid(FileSelectFileInfo file)
    {
        return !(file.InvalidExtension || file.InvalidMaxFileSize || file.InvalidMinFileSize);
    }

    protected void UpdateValidationModel()
    {
        bool areAllUploadedFilesValid = false;

        if (FilesValidationInfo.Keys.Count > 0 &&
            !FilesValidationInfo.Values.Contains(false))
        {
            areAllUploadedFilesValid = true;
        }

        SelectedItem.IsValidIdAccepted = areAllUploadedFilesValid;

        StateHasChanged();
    }
}
