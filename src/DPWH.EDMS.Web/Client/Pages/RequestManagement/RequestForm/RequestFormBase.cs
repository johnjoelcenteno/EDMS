﻿using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Telerik.Blazor.Components;
using System.Linq;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using Microsoft.AspNetCore.Components.Authorization;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using AutoMapper;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.RequestForm;

public class RequestFormBase : RxBaseComponent
{
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required IDocumentService DocumentService { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    [Inject] public required IDataLibraryService DataLibraryService { get; set; }
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }

    [Parameter] public bool IsEditMode { get; set; } = false;
    [Parameter] public EventCallback<(CreateRecordRequest, UploadSupportingFileRequestModel, UploadSupportingFileRequestModel)> HandleCreateOnSubmit { get; set; }
    //[Parameter] public EventCallback<CreateRecordRequest> HandleEditOnSubmit { get; set; }
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    protected CreateRecordRequest SelectedItem { get; set; } = new();

    protected List<string> DocumentClaimants { get; set; } = new();
    //protected List<string> allRecords = new List<string> { "Record 1", "Record 2", "Record 3" };
    //protected List<string> validIdTypes = new List<string> { "ID Type 1", "ID Type 2", "ID Type 3" };
    //protected List<string> supportingDocumentTypes = new List<string> { "Document Type 1", "Document Type 2", "Document Type 3" };
    protected List<GetValidIDsResult> ValidIDsList = new();
    protected List<GetAuthorizationDocumentsResult> AuthorizedDocTypeList = new();
    protected List<GetRecordTypesResult> RecordTypesList = new();

    protected List<Guid> SelectedRecordTypesIdList = new();
    protected List<ValidationResult> ValidationErrors { get; set; } = new();
    protected TelerikDropDownList<GetValidIDsResult, string> ValidIDDropRef = new();
    protected TelerikDropDownList<GetSecondaryIDsResult, string> SecondaryIDDropRef = new();
    protected TelerikDropDownList<GetRecordTypesResult, string> RecordTypesDropRef = new();

    protected FluentValidationValidator? FluentValidationValidator;

    protected Guid? SelectedValidIdTypeId { get; set; }
    protected Guid? SelectedAuthorizedDocTypeId { get; set; }
    protected UploadSupportingFileRequestModel? SelectedValidId { get; set; }
    protected UploadSupportingFileRequestModel? SelectedAuthorizedDoc { get; set; }
    //protected TelerikUpload SupportedDocumentUploadRef { get; set; }

    public int MinFileSize { get; set; } = 1024;
    public int MaxFileSize { get; set; } = 4 * 1024 * 1024;
    public List<string> AllowedExtensions { get; set; } = new List<string>() { ".docx", ".pdf" };
    Dictionary<string, bool> FilesValidationInfo { get; set; } = new Dictionary<string, bool>();

    protected override void OnInitialized()
    {
        LoadClaimantTypes();
        _SetDefaultRequestDate();
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await LoadValidIDTypes();
        await LoadAuthorizedDocumentTypes();
        await LoadRecordTypes();        
        IsLoading = false;
    }

    #region Load Events
    private void _SetDefaultRequestDate()
    {
        if (SelectedItem.DateRequested == default)
        {
            SelectedItem.DateRequested = DateTime.Now;
        }
    }
    protected async Task LoadValidIDTypes()
    {
        var validIdResult = await LookupsService.GetValidIdTypes();

        if (validIdResult.Success)
        {
            ValidIDsList = validIdResult.Data.ToList();
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading valid ids");
        }
    }
    protected async Task LoadAuthorizedDocumentTypes()
    {
        // TO be renamed
        var authDocTypeResult = await LookupsService.GetAuthorizationDocumentTypes();

        if (authDocTypeResult.Success)
        {
            AuthorizedDocTypeList = authDocTypeResult.Data.ToList();
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading supporting documents");
        }
    }
    protected async Task LoadRecordTypes()
    {
        var recordTypesResult = await LookupsService.GetRecordTypes();

        if (recordTypesResult.Success)
        {
            RecordTypesList = recordTypesResult.Data.ToList();
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading record types");
        }
    }
    protected void LoadClaimantTypes()
    {
        DocumentClaimants = Enum.GetValues(typeof(ClaimantTypes))
               .Cast<ClaimantTypes>()
               .Select(e => e.ToString())
               .ToList();
    }
    #endregion

    #region Submit Events
    protected async Task HandleOnSubmitCallback()
    {
        if (await FluentValidationValidator!.ValidateAsync())
        {
            if (HandleCreateOnSubmit.HasDelegate)
            {
                await HandleCreateOnSubmit.InvokeAsync((SelectedItem, SelectedValidId, SelectedAuthorizedDoc)!);
            }
        }
        else
        {
            //ToastService!.ShowError("Something went wrong, on submitting form. Please contact administrator.");
            await HandleCreateOnSubmit.InvokeAsync((null, null, null)!);
        }
    }
    protected async Task HandleOnCancelCallback()
    {
        if (HandleOnCancel.HasDelegate)
        {
            await HandleOnCancel.InvokeAsync();
        }
    }
    #endregion

    #region Uploads
    protected async void OnSelectValidId(FileSelectEventArgs args)
    {
        if (SelectedItem != null && GenericHelper.IsGuidHasValue(SelectedValidIdTypeId))
        {
            SelectedValidId = new UploadSupportingFileRequestModel()
            {
                document = null!,
                documentType = Api.Contracts.RecordRequestProvidedDocumentTypes.ValidId,
                documentTypeId = SelectedValidIdTypeId
            };

            SelectedValidId.document = await DocumentService.GetFileToUpload(args);
        }
    }

    protected void OnRemoveValidId(FileSelectEventArgs args)
    {
        SelectedValidId = null;
    }

    protected async void OnSelectSuppDoc(FileSelectEventArgs args)
    {
        if (SelectedItem != null && GenericHelper.IsGuidHasValue(SelectedAuthorizedDocTypeId))
        {
            SelectedAuthorizedDoc = new UploadSupportingFileRequestModel()
            {
                document = null!,
                documentType = Api.Contracts.RecordRequestProvidedDocumentTypes.AuthorizationDocument,
                documentTypeId = SelectedAuthorizedDocTypeId
            };

            SelectedAuthorizedDoc.document = await DocumentService.GetFileToUpload(args);
        }
    }

    protected void OnRemoveSuppDoc(FileSelectEventArgs args)
    {
        SelectedAuthorizedDoc = null;
    }

    //protected bool IsSelectedFileValid(FileSelectFileInfo file)
    //{
    //    return !(file.InvalidExtension || file.InvalidMaxFileSize || file.InvalidMinFileSize);
    //}

    //protected void UpdateValidationModel()
    //{
    //    bool areAllUploadedFilesValid = false;

    //    if (FilesValidationInfo.Keys.Count > 0 &&
    //        !FilesValidationInfo.Values.Contains(false))
    //    {
    //        areAllUploadedFilesValid = true;
    //    }

    //    SelectedItem.IsValidIdAccepted = areAllUploadedFilesValid;

    //    StateHasChanged();
    //}

    #endregion
    //protected async Task<CreateRecordRequest> FetchDocumentRequestValue()
    //{
    //    List<Guid> parsedGuids = new List<Guid>();
    //    foreach (var record in SelectedItem.RecordsRequested)
    //    {
    //        if (Guid.TryParse(record, out var guid))
    //        {
    //            parsedGuids.Add(guid);
    //        }
    //        else
    //        {
    //            ToastService!.ShowError($"Warning: Invalid GUID format for record: {record}");
    //        }
    //    }

    //    return new CreateRecordRequest
    //    {
    //        EmployeeNumber = SelectedItem.EmployeeNo,
    //        ControlNumber = SelectedItem.ControlNumber,
    //        IsActiveEmployee = SelectedItem.IsActive,
    //        Claimant = SelectedItem.DocumentClaimant.ToString(),
    //        DateRequested = SelectedItem.DateRequested,
    //        AuthorizedRepresentative = SelectedItem.AuthorizedRepresentative,
    //        //ValidId = SelectedItem.ValidId, // TODO
    //        //SupportingDocument = SelectedItem,
    //        //RequestedRecords = SelectedItem.RecordsRequested?.Select(Guid.Parse).ToList(), // TODO
    //        RequestedRecords = parsedGuids,
    //        Purpose = "Visa Application"
    //    };
    //}
}