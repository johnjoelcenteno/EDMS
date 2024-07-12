using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequestForm;

public class ViewRequestFormBase : RequestDetailsOverviewBase
{
    [Inject] public required NavigationManager NavigationManager { get; set; }
    protected UpdateResponseBaseApiResponse? updateResponse;
    protected int ActiveTabIndex { get; set; } = 0;
    protected int CurrentStepIndex { get; set; }
    protected DateTime? DateReceived { get; set; } = DateTime.Now;
    protected DateTime? TimeReceived { get; set; } = DateTime.Now;
    protected bool IsModalVisible { get; set; }
    public int MinFileSize { get; set; } = 1024;
    public int MaxFileSize { get; set; } = 4 * 1024 * 1024;
    public List<string> AllowedExtensions { get; set; } = new List<string>() { ".docx", ".pdf" };

    protected override void OnParametersSet()
    {
        CancelReturnUrl = "/request-management";
    }

    protected async override Task OnInitializedAsync()
    {

        IsLoading = true;

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

    protected async Task OnSubmit(string newStatus, int tabIndex)
    {
        IsLoading = true;
        IsModalVisible = false;

        var request = new UpdateRecordRequestStatus
        {
            Id = SelectedRecordRequest.Id,
            Status = newStatus
        };

        updateResponse = await RequestManagementService.UpdateStatus(request);
        if (updateResponse.Success)
        {
            SelectedRecordRequest.Status = newStatus;
            UpdateCurrentStepIndex();
        }

        ActiveTabIndex = tabIndex;
        StateHasChanged();
        IsLoading = false;
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
