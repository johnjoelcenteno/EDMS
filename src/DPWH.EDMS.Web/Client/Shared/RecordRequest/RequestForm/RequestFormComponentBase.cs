using AutoMapper;
using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
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
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }

    [Parameter] public bool IsEditMode { get; set; } = false;
    [Parameter] public EventCallback<(CreateRecordRequest, UploadRecordRequestDocumentModel, UploadRecordRequestDocumentModel)> HandleCreateOnSubmit { get; set; }
    //[Parameter] public EventCallback<CreateRecordRequest> HandleEditOnSubmit { get; set; } // TODO
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    protected CreateRecordRequest SelectedItem { get; set; } = new();

    // Lists
    protected List<string> DocumentClaimants { get; set; } = new();
    protected List<GetValidIDsResult> ValidIDsList = new();
    protected List<GetAuthorizationDocumentsResult> AuthorizeDocTypeList = new();
    protected List<GetLookupResult> IssuanceList = new();
    protected List<GetLookupResult> EmployeeRecordList = new();

    protected List<Guid> SelectedIssuanceList = new();
    protected List<Guid> SelectedEmployeeRecordList = new();

    // Dropdowns
    protected TelerikDropDownList<GetValidIDsResult, string> ValidIDDropRef = new();
    protected TelerikDropDownList<GetAuthorizationDocumentsResult, string> AuthorizationDocumentDropRef = new();
    protected TelerikDropDownList<GetLookupResult, string> IssuanceDropRef = new();
    protected TelerikDropDownList<GetLookupResult, string> EmployeeRecordDropRef = new();

    // For Uploads
    protected Guid? SelectedValidIdTypeId { get; set; }
    protected Guid? SelectedAuthorizedDocTypeId { get; set; }
    protected UploadRecordRequestDocumentModel? SelectedValidId { get; set; }
    protected UploadRecordRequestDocumentModel? SelectedAuthorizedDocument { get; set; }

    // Validator
    protected FluentValidationValidator? FluentValidationValidator;

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
        var suppTypeResult = await LookupsService.GetAuthorizationDocuments();

        if (suppTypeResult.Success)
        {
            AuthorizeDocTypeList = suppTypeResult.Data.ToList();
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading authorize document");
        }
    }
    protected async Task LoadIssuanceList()
    {
        var res = await LookupsService.GetIssuances();

        if (res.Success)
        {
            IssuanceList = res.Data.ToList();
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading issuance list.");
        }
    }

    protected async Task LoadEmployeeRecordList()
    {
        var res = await LookupsService.GetEmployeeRecords();

        if (res.Success)
        {
            EmployeeRecordList = res.Data.ToList();
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading employee records.");
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

    protected void HandleRequestRecords()
    {
        HashSet<Guid> set = new HashSet<Guid>();

        set.UnionWith(SelectedIssuanceList);
        set.UnionWith(SelectedEmployeeRecordList);

        SelectedItem.RequestedRecords = set;
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
