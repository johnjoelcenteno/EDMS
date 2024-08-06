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
    protected GetTransmittalReceiptModelBaseApiResponse GetTransmittalRecceipt { get; set; } = new();
    protected UpdateResponseBaseApiResponse? UpdateResponse;
    protected GetUserByIdResult User = new GetUserByIdResult();
    protected int ActiveTabIndex { get; set; } = 0;
    protected int ProgressIndex { get; set; }
    protected int MinFileSize { get; set; } = 1024;
    protected int MaxFileSize { get; set; } = 4 * 1024 * 1024;
    protected bool HasNoRecords { get; set; } = false;
    protected bool IsModalVisible { get; set; }
    protected bool IsRecordUploadEnabled { get; set; } = false;
    protected List<string> AllowedExtensions { get; set; } = new List<string>() { ".docx", ".pdf" };
    protected DateTimeOffset DateReceived { get; set; } = DateTimeOffset.Now;
    protected DateTimeOffset TimeReceived { get; set; } = DateTimeOffset.Now;
    protected DateTime MaxDate = DateTime.Now;

    // For Uploads
    protected Dictionary<Guid, UploadRequestedRecordDocumentModel> UploadRequestedRecords { get; set; } = new();
    protected UploadTransmittalReceiptDocumentModel? SelectedTransmittalReceipt { get; set; }

    // Validation
    protected string? RequestedRecordValidation { get; set; }
    protected string? TransmittalValidation { get; set; }

    // Placeholders
    protected string? Remarks { get; set; }
    protected TelerikRadioGroup<ListItem, int?>? RadioGroupRef { get; set; }
    protected int? RadioGroupValue { get; set; }

    protected List<ListItem> RadioGroupData { get; set; } = new List<ListItem>()
    {
        new ListItem { Id = 1, Text = "TC" },
        new ListItem { Id = 2, Text = "MC" }
    };

    protected class ListItem
    {
        public int Id { get; set; }
        public string? Text { get; set; }
    }

    protected override void OnParametersSet()
    {
        CancelReturnUrl = "/request-management";
    }

    protected async override Task OnInitializedAsync()
    {

        IsLoading = true;
        await FetchUser();
        await GetTransmittalData();

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

        UpdateProgressIndex();
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

    protected async Task GetTransmittalData()
    {
        try
        {
            GetTransmittalRecceipt = await RecordRequestSupportingFilesService.GetTransmittalReceipt(Guid.Parse(RequestId));
        }
        catch (Exception)
        {

        }
    }

    protected string PickUpLocation
    {
        get
        {
            if (User.UserAccess == "Manager" || User.UserAccess == "Staff")
            {
                if (User.Office == Offices.RMD.ToString())
                {
                    return "Records Management Division";
                }
                else
                {
                    return "Human Resource Management Division";
                }
            }
            else
            {
                return "Records Management Division";
            }
        }
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
            UpdateProgressIndex();
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
        RequestedRecordValidation = string.Empty;
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
            RequestedRecordValidation = "Please select a file for each requested record.";
        }
    }

    protected void ValidateTransmittalFileSelect()
    {
        TransmittalValidation = string.Empty;

        if (SelectedTransmittalReceipt != null && SelectedTransmittalReceipt?.Document != null)
        {
            IsModalVisible = true;
        }
        else
        {
            TransmittalValidation = "Transmittal Receipt is required.";
        }
    }

    protected async Task OnReview()
    {
        IsLoading = true;
        IsModalVisible = false;

        if (UploadRequestedRecords != null && UploadRequestedRecords.Count > 0)
        {
            foreach (var uploadRecord in UploadRequestedRecords.Values)
            {
                if (uploadRecord.Document != null)
                {
                    await RecordRequestSupportingFilesService.UploadRequestedRecord(uploadRecord.Document, uploadRecord.Id);
                }
            }
        }

        await OnStatusChange(RecordRequestStates.Reviewed.ToString());

        await LoadData((res) =>
        {
            SelectedRecordRequest = res;

        });

        StateHasChanged();
        IsLoading = false;
    }

    protected async Task OnApprove()
    {
        IsLoading = true;
        IsModalVisible = false;
        await OnStatusChange(RecordRequestStates.Approved.ToString());
        StateHasChanged();
        IsLoading = false;
    }

    protected async Task OnRelease()
    {
        IsLoading = true;
        IsModalVisible = false;
        await OnStatusChange("For Release");
        StateHasChanged();
        IsLoading = false;
    }

    protected async Task OnClaim()
    {
        IsLoading = true;
        IsModalVisible = false;

        if (SelectedTransmittalReceipt?.Document != null)
        {
            await RecordRequestSupportingFilesService.UploadTransmittalReceipt(DateReceived, TimeReceived, SelectedTransmittalReceipt.Document, SelectedTransmittalReceipt.RecordRequestId);
        }

        await OnStatusChange(RecordRequestStates.Claimed.ToString());
        NavigationManager.NavigateTo("/request-management");
        StateHasChanged();
        IsLoading = false;
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
                RequestedRecordValidation = string.Empty;
            }
            else if (User.Office == Offices.HRMD.ToString() && HRMDRecords != null && HRMDRecords.All(r => UploadRequestedRecords.ContainsKey(r.Id) && UploadRequestedRecords[r.Id].Document != null))
            {
                RequestedRecordValidation = string.Empty;
            }
            else if (SelectedRecordRequest.RequestedRecords.All(r => UploadRequestedRecords.ContainsKey(r.Id) && UploadRequestedRecords[r.Id].Document != null))
            {
                RequestedRecordValidation = string.Empty;
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

    protected async void OnSelectTransmittalReceipt(FileSelectEventArgs args)
    {
        SelectedTransmittalReceipt = new UploadTransmittalReceiptDocumentModel()
        {
            Document = null!,
            RecordRequestId = Guid.Parse(RequestId)
        };

        SelectedTransmittalReceipt.Document = await DocumentService.GetFileToUpload(args);
        TransmittalValidation = string.Empty;

        StateHasChanged();
    }

    protected void OnRemoveTransmittalReceipt(FileSelectEventArgs args)
    {
        SelectedTransmittalReceipt = null;
    }

    protected void UpdateProgressIndex()
    {
        switch (SelectedRecordRequest.Status)
        {
            case var status when status == RecordRequestStates.Submitted.ToString():
                ProgressIndex = 0;
                ActiveTabIndex = 1;
                IsRecordUploadEnabled = true;
                break;
            case var status when status == RecordRequestStates.Reviewed.ToString():
                ProgressIndex = 1;
                ActiveTabIndex = 2;
                IsRecordUploadEnabled = false;
                break;
            case var status when status == RecordRequestStates.Approved.ToString():
                ProgressIndex = 2;
                ActiveTabIndex = 3;
                break;
            case var status when status == RecordRequestStates.ForRelease.ToString() || SelectedRecordRequest.Status == "For Release":
                ProgressIndex = 3;
                ActiveTabIndex = 4;
                break;
            case var status when status == RecordRequestStates.Claimed.ToString():
                ProgressIndex = 4;
                ActiveTabIndex = 0;
                break;
            default:
                break;
        }
    }

    public void ValueChangeHandler(int newStep)
    {

    }
}
