using AutoMapper;
using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;

public class RequestFormComponentBase : RxBaseComponent
{
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required IDocumentService DocumentService { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    [Inject] public required IDataLibraryService DataLibraryService { get; set; }
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }

    [Parameter] public bool IsEditMode { get; set; } = false;
    [Parameter] public EventCallback<(CreateRecordRequest, UploadRecordRequestDocumentModel, UploadRecordRequestDocumentModel)> HandleCreateOnSubmit { get; set; }
    //[Parameter] public EventCallback<CreateRecordRequest> HandleEditOnSubmit { get; set; } // TODO
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    protected CreateRecordRequest SelectedItem { get; set; } = new();

    protected List<string> DocumentClaimants { get; set; } = new();
    protected List<GetValidIDsResult> ValidIDsList = new();
    protected List<GetAuthorizationDocumentsResult> AuthorizeDocTypeList = new();
    protected List<GetRecordTypesResult> RecordTypesList = new();

    protected List<Guid> SelectedRecordTypesIdList = new();

    protected TelerikDropDownList<GetValidIDsResult, string> ValidIDDropRef = new();
    protected TelerikDropDownList<GetSecondaryIDsResult, string> SecondaryIDDropRef = new();
    protected TelerikDropDownList<GetRecordTypesResult, string> RecordTypesDropRef = new();

    protected FluentValidationValidator? FluentValidationValidator;

    protected Guid? SelectedValidIdTypeId { get; set; }
    protected Guid? SelectedAuthorizedDocTypeId { get; set; }
    protected UploadRecordRequestDocumentModel? SelectedValidId { get; set; }
    protected UploadRecordRequestDocumentModel? SelectedAuthorizedDocument { get; set; }
    protected UserModel CurrentUser { get; set; } = new();
    protected string UserFullname = string.Empty;

    public int MinFileSize { get; set; } = 1024;
    public int MaxFileSize { get; set; } = 4 * 1024 * 1024;
    public List<string> AllowedExtensions { get; set; } = new List<string>() { ".docx", ".pdf" };
    Dictionary<string, bool> FilesValidationInfo { get; set; } = new Dictionary<string, bool>();

    #region Load Events
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
    protected async Task LoadAuthorizeDocumentTypes()
    {
        // TO be renamed
        var suppTypeResult = await LookupsService.GetAuthorizationDocumentTypes();

        if (suppTypeResult.Success)
        {
            AuthorizeDocTypeList = suppTypeResult.Data.ToList();
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading authorize document");
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
        if (await FluentValidationValidator!.ValidateAsync() && IsValidIdValid() && IsAuthorizedDocumentValid())
        {
            if (HandleCreateOnSubmit.HasDelegate)
            {
                await HandleCreateOnSubmit.InvokeAsync((SelectedItem, SelectedValidId, SelectedAuthorizedDocument)!);
            }
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
            SelectedValidId = new UploadRecordRequestDocumentModel()
            {
                Document = null!,
                DocumentType = Api.Contracts.RecordRequestProvidedDocumentTypes.ValidId,
                DocumentTypeId = SelectedValidIdTypeId
            };

            SelectedValidId.Document = await DocumentService.GetFileToUpload(args);
        }
    }

    protected void OnRemoveValidId(FileSelectEventArgs args)
    {
        SelectedValidId = null;
    }

    protected async void OnSelectAuthorizedDocument(FileSelectEventArgs args)
    {
        if (SelectedItem != null && GenericHelper.IsGuidHasValue(SelectedAuthorizedDocTypeId))
        {
            SelectedAuthorizedDocument = new UploadRecordRequestDocumentModel()
            {
                Document = null!,
                DocumentType = Api.Contracts.RecordRequestProvidedDocumentTypes.AuthorizationDocument,
                DocumentTypeId = SelectedAuthorizedDocTypeId
            };

            SelectedAuthorizedDocument.Document = await DocumentService.GetFileToUpload(args);
        }
    }

    protected void OnRemoveAuthorizedDocument(FileSelectEventArgs args)
    {
        SelectedAuthorizedDocument = null;
    }

    protected bool IsValidIdValid()
    {
        return
            SelectedItem.Claimant != ClaimantTypes.AuthorizedRepresentative.ToString() ||
            SelectedValidId != null &&
            SelectedValidId.Document != null &&
            SelectedValidId.DocumentTypeId != null &&
            SelectedValidId.DocumentType != null;
    }

    protected bool IsAuthorizedDocumentValid()
    {
        return
            SelectedItem.Claimant != ClaimantTypes.AuthorizedRepresentative.ToString() ||
            SelectedAuthorizedDocument != null &&
            SelectedAuthorizedDocument.Document != null &&
            SelectedAuthorizedDocument.DocumentTypeId != null &&
            SelectedAuthorizedDocument.DocumentType != null;
    }
    #endregion
}
