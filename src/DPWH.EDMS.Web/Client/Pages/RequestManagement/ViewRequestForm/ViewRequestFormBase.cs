using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequestForm;

public class ViewRequestFormBase : RequestDetailsOverviewBase
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required IDocumentService DocumentService { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    [Inject] public required IRecordRequestSupportingFilesService RecordRequestSupportingFilesService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }

    protected UpdateResponseBaseApiResponse? UpdateResponse;
    protected GetUserByIdResult User = new GetUserByIdResult();
    protected int ActiveTabIndex { get; set; } = 0;
    protected int CurrentStepIndex { get; set; }
    protected bool IsModalVisible { get; set; }
    protected int MinFileSize { get; set; } = 1024;
    protected int MaxFileSize { get; set; } = 4 * 1024 * 1024;
    protected bool HasNoRecords { get; set; } = false;
    protected List<string> AllowedExtensions { get; set; } = new List<string>() { ".docx", ".pdf" };

    // For Uploads
    protected Dictionary<Guid, UploadRequestedRecordDocumentModel> UploadRequestedRecords { get; set; } = new();
    protected string? ValidationMessage { get; set; }

    // Placeholders
    protected string? Remarks { get; set; }
    protected string PickUpLocation
    {
        get
        {
            if (User.UserAccess == "Manager" || User.UserAccess == "Staff")
            {
                return User.Office;
            }
            else
            {
                return Offices.RMD.ToString();
            }
        }
    }
    protected DateTime? DateReceived { get; set; } = DateTime.Now;
    protected DateTime? TimeReceived { get; set; } = DateTime.Now;

    protected override void OnParametersSet()
    {
        CancelReturnUrl = "/request-management";
    }

    protected async override Task OnInitializedAsync()
    {

        IsLoading = true;
        await FetchUser();

        await LoadData((res) =>
        {
            SelectedRecordRequest = res;
            BreadcrumbItems.AddRange(new List<BreadcrumbModel>
            {
                new BreadcrumbModel
                {
                    Icon = "menu",
                    Text = "Request Management",
                    Url = "/request-management",
                },
                new BreadcrumbModel
                {
                    Icon = "create_new_folder",
                    Text = "View Request Form",
                    Url = NavManager.Uri.ToString(),
                },
            });
        });

        UpdateCurrentStepIndex();
        IsLoading = false;
    }

    protected async Task OnStatusChange(string newStatus)
    {
        var request = new UpdateRecordRequestStatus
        {
            Id = SelectedRecordRequest.Id,
            Status = newStatus
        };

        UpdateResponse = await RequestManagementService.UpdateStatus(request);
        if (UpdateResponse.Success)
        {
            SelectedRecordRequest.Status = newStatus;
            UpdateCurrentStepIndex();
        }
    }

    protected async Task UpdateIsAvailable(RequestedRecordModel record)
    {
        try
        {
            var response = await RequestManagementService.UpdateIsAvailable(record.IsAvailable, new List<Guid> { record.Id });

            if (!response.Success)
            {
                // Handle unsuccessful update
                // Optionally revert the IsAvailable state if needed
            }
        }
        catch (Exception ex)
        {
            // Handle exception
            // Optionally revert the IsAvailable state if needed
        }
    }

    protected void ValidateRecordsFileSelect()
    {
        ValidationMessage = string.Empty;
        bool allFilesSelected = true;

        if (User.Office == Offices.RMD.ToString() && RMDRecords != null)
        {
            foreach (var record in RMDRecords)
            {
                
                if (!record.IsAvailable)
                {
                    allFilesSelected = true;
                }
                else
                {
                    if (!UploadRequestedRecords.ContainsKey(record.Id) || UploadRequestedRecords[record.Id].Document == null)
                    {
                        allFilesSelected = false;
                        break;
                    }
                }
            }
        }
        else if (User.Office == Offices.HRMD.ToString() && HRMDRecords != null)
        {
            foreach (var record in HRMDRecords)
            {
                
                if (!record.IsAvailable)
                {
                    allFilesSelected = true;
                }
                else
                {
                    if (!UploadRequestedRecords.ContainsKey(record.Id) || UploadRequestedRecords[record.Id].Document == null)
                    {
                        allFilesSelected = false;
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (var record in SelectedRecordRequest.RequestedRecords)
            {
                
                if (!record.IsAvailable)
                {
                    allFilesSelected = true;
                }
                else
                {
                    if (!UploadRequestedRecords.ContainsKey(record.Id) || UploadRequestedRecords[record.Id].Document == null)
                    {
                        allFilesSelected = false;
                        break;
                    }
                }
            }
        }
        
        if (allFilesSelected)
        {
            IsModalVisible = true;
        }
        else
        {
            ValidationMessage = "Please select a file for each requested record.";
        }
    }

    protected async Task OnUploadDocument()
    {
        foreach (var uploadRecord in UploadRequestedRecords.Values)
        {
            if (uploadRecord.Document != null)
            {
                await RecordRequestSupportingFilesService.UploadRequestedRecord(uploadRecord.Document, uploadRecord.Id);
            }
        }
    }

    protected async Task OnReview()
    {
        IsLoading = true;
        IsModalVisible = false;
        await OnStatusChange(RecordRequestStates.Release.ToString());

        foreach (var record in SelectedRecordRequest.RequestedRecords)
        {
            await UpdateIsAvailable(record);
        }

        ActiveTabIndex = 2;
        StateHasChanged();
        IsLoading = false;
    }

    protected async Task OnRelease()
    {
        IsLoading = true;
        IsModalVisible = false;
        await OnUploadDocument();
        await OnStatusChange(RecordRequestStates.Claimed.ToString());
        ActiveTabIndex = 3;
        StateHasChanged();
        IsLoading = false;
    }

    protected void OnClaim()
    {
        IsLoading = true;
        IsModalVisible = false;
        NavigationManager.NavigateTo("/request-management");
        StateHasChanged();
        IsLoading = false;
    }

    protected async Task FetchUser()
    {
        var authState = await AuthenticationStateAsync!;
        var user = authState.User;
        var userId = user.GetUserId();

        var userRes = await UsersService.GetById(userId);

        if (userRes.Success)
        {
            User.UserAccess = userRes.Data.UserAccess;
            User.Office = userRes.Data.Office;
        }
        else
        {
            ToastService.ShowError("Something went wrong on fetching user data");
        }
    }

    private void UpdateCurrentStepIndex()
    {
        switch (SelectedRecordRequest.Status)
        {
            case var status when status == RecordRequestStates.Review.ToString():
                CurrentStepIndex = 1;
                break;

            case var status when status == RecordRequestStates.Release.ToString():
                CurrentStepIndex = 2;
                break;

            default:
                CurrentStepIndex = 3;
                break;
        }
    }

    protected async void OnSelectDocument(FileSelectEventArgs args, Guid recordId)
    {
        var requestedRecord = SelectedRecordRequest.RequestedRecords.FirstOrDefault(r => r.Id == recordId);

        if (requestedRecord != null)
        {
            var document = await DocumentService.GetFileToUpload(args);

            if (UploadRequestedRecords.ContainsKey(recordId))
            {
                UploadRequestedRecords[recordId].Document = document;
            }
            else
            {
                UploadRequestedRecords[recordId] = new UploadRequestedRecordDocumentModel()
                {
                    Document = document,
                    Id = recordId
                };
            }

            if (User.Office == Offices.RMD.ToString() && RMDRecords != null && RMDRecords.All(r => UploadRequestedRecords.ContainsKey(r.Id) && UploadRequestedRecords[r.Id].Document != null))
            {
                ValidationMessage = string.Empty;
            }
            else if (User.Office == Offices.HRMD.ToString() && HRMDRecords != null && HRMDRecords.All(r => UploadRequestedRecords.ContainsKey(r.Id) && UploadRequestedRecords[r.Id].Document != null))
            {
                ValidationMessage = string.Empty;
            }
            else if (SelectedRecordRequest.RequestedRecords.All(r => UploadRequestedRecords.ContainsKey(r.Id) && UploadRequestedRecords[r.Id].Document != null))
            {
                ValidationMessage = string.Empty;
            }
        }

        StateHasChanged();
    }

    protected void OnRemoveDocument(FileSelectEventArgs args, Guid recordId)
    {
        if (UploadRequestedRecords.ContainsKey(recordId))
        {
            UploadRequestedRecords.Remove(recordId);
        }
    }


    protected async void OnSelectTransmittal(FileSelectEventArgs args)
    {

    }

    protected async void OnRemoveTransmittal(FileSelectEventArgs args)
    {

    }

    public void ValueChangeHandler(int newStep)
    {

    }
}
