using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.PendingRequests.RequestForm;

public class RequestFormBase : RxBaseComponent
{
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required IDocumentService DocumentService { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }

    [Parameter] public bool IsEditMode { get; set; } = false;
    [Parameter] public EventCallback<DocumentRequestModel> HandleOnSubmit { get; set; }
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    protected DocumentRequestModel SelectedItem { get; set; } = new();

    protected List<DocumentClaimant> documentClaimants = Enum.GetValues(typeof(DocumentClaimant)).Cast<DocumentClaimant>().ToList();
    //protected List<string> allRecords = new List<string> { "Record 1", "Record 2", "Record 3" };
    //protected List<string> validIdTypes = new List<string> { "ID Type 1", "ID Type 2", "ID Type 3" };
    //protected List<string> supportingDocumentTypes = new List<string> { "Document Type 1", "Document Type 2", "Document Type 3" };
    protected List<GetValidIDsResult> ValidIDsList = new();
    protected List<GetSecondaryIDsResult> SecondaryIDsList = new();
    protected List<GetRecordTypesResult> RecordTypesList = new();
    protected List<ValidationResult> ValidationErrors { get; set; } = new();
    protected TelerikDropDownList<GetValidIDsResult, string> ValidIDDropRef = new();
    protected TelerikDropDownList<GetSecondaryIDsResult, string> SecondaryIDDropRef = new();
    protected TelerikDropDownList<GetRecordTypesResult, string> RecordTypesDropRef = new();

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

    protected override async Task OnInitializedAsync()
    {
        await GetValidIDTypes();
        await GetSecondaryIDTypes();
        await GetRecordTypes();
    }

    protected async Task GetValidIDTypes()
    {
        var validIdResult = await LookupsService.GetValidIdTypes();
        if (validIdResult != null)
        {
            ValidIDsList = validIdResult.Data.Select(item => new GetValidIDsResult
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }
    }

    protected async Task GetSecondaryIDTypes()
    {
        var secondaryIdResult = await LookupsService.GetSecondaryIdTypes();
        if (secondaryIdResult != null)
        {
            SecondaryIDsList = secondaryIdResult.Data.Select(item => new GetSecondaryIDsResult
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }
    }

    protected async Task GetRecordTypes()
    {
        var recordTypesResult = await LookupsService.GetRecordTypes();
        if (recordTypesResult != null)
        {
            RecordTypesList = recordTypesResult.Data.Select(item => new GetRecordTypesResult
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();
        }
    }


    protected async Task HandleOnSubmitCallback()
    {
        if (await FluentValidationValidator!.ValidateAsync())
        {
            var createRecordRequest = await FetchDocumentRequestValue();

            if (createRecordRequest != null)
            {
                var createRes = await RecordRequestsService!.CreateRecordRequest(createRecordRequest);

                if (createRes.Success)
                {
                    ToastService!.ShowSuccess("Successfully created address!");
                }
                else
                {
                    ToastService!.ShowError($"Something went wrong on creating address! {createRes.Error}");
                }
                if (HandleOnSubmit.HasDelegate)
                {
                    await HandleOnSubmit.InvokeAsync(SelectedItem);
                    ToastService!.ShowSuccess("Successfully created request!");
                }
            }

            //var createRes = await RecordRequestsService!.CreateRecordRequest(SelectedItem);

            //if (createRes.Success)
            //{
            //    ToastService!.ShowSuccess("Successfully created address!");
            //}
            //else
            //{
            //    ToastService!.ShowError($"Something went wrong on creating address! {createRes.Error}");
            //}
            //if (HandleOnSubmit.HasDelegate)
            //{
            //    await HandleOnSubmit.InvokeAsync(SelectedItem);
            //    ToastService!.ShowSuccess("Successfully created request!");
            //}
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

    protected async Task<CreateRecordRequest> FetchDocumentRequestValue()
    {
        List<Guid> parsedGuids = new List<Guid>();
        foreach (var record in SelectedItem.RecordsRequested)
        {
            if (Guid.TryParse(record, out var guid))
            {
                parsedGuids.Add(guid);
            }
            else
            {
                ToastService!.ShowError($"Warning: Invalid GUID format for record: {record}");
            }
        }

        return new CreateRecordRequest
        {
            EmployeeNumber = SelectedItem.EmployeeNo,
            ControlNumber = SelectedItem.ControlNumber,
            IsActiveEmployee = SelectedItem.IsActive,
            Claimant = SelectedItem.DocumentClaimant.ToString(),
            DateRequested = SelectedItem.DateRequested,
            AuthorizedRepresentative = SelectedItem.AuthorizedRepresentative,
            //ValidId = SelectedItem.ValidId, // TODO
            //SupportingDocument = SelectedItem,
            //RequestedRecords = SelectedItem.RecordsRequested?.Select(Guid.Parse).ToList(), // TODO
            RequestedRecords = parsedGuids,
            Purpose = "Visa Application"
        };
    }
}
