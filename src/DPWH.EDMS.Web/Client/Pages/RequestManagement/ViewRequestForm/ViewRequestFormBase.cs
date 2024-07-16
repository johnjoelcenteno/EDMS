using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequestForm;

public class ViewRequestFormBase : RequestDetailsOverviewBase
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    protected UpdateResponseBaseApiResponse? UpdateResponse;
    protected string Office { get; set; } = string.Empty;
    protected int ActiveTabIndex { get; set; } = 0;
    protected int CurrentStepIndex { get; set; }
    protected DateTime? DateReceived { get; set; } = DateTime.Now;
    protected DateTime? TimeReceived { get; set; } = DateTime.Now;
    protected bool IsModalVisible { get; set; }
    protected int MinFileSize { get; set; } = 1024;
    protected int MaxFileSize { get; set; } = 4 * 1024 * 1024;
    protected List<string> AllowedExtensions { get; set; } = new List<string>() { ".docx", ".pdf" };

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
            Office = userRes.Data.Office;
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

    protected async void OnSelectDocument(FileSelectEventArgs args)
    {

    }

    protected async void OnRemoveDocument(FileSelectEventArgs args)
    {

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
