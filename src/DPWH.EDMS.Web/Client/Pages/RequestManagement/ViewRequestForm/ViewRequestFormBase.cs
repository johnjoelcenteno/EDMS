using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequestForm;

public class ViewRequestFormBase : RequestDetailsOverviewBase
{
    [Inject] public required NavigationManager NavigationManager { get; set; }
    protected int ActiveTabIndex { get; set; } = 0;
    protected int CurrentStepIndex { get; set; } = 1;
    protected string Status { get; set; } = "Submitted";
    protected DateTime? DateReceived { get; set; } = DateTime.Now;
    protected DateTime? TimeReceived { get; set; } = DateTime.Now;
    protected bool IsModalVisible { get; set; }
    public int MinFileSize { get; set; } = 1024;
    public int MaxFileSize { get; set; } = 4 * 1024 * 1024;
    public List<string> AllowedExtensions { get; set; } = new List<string>() { ".docx", ".pdf" };

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

        IsLoading = false;
        StateHasChanged();
    }

    public void ValueChangeHandler(int newStep)
    {

    }

    protected override void OnParametersSet()
    {
        CancelReturnUrl = "/request-management";
    }

    protected void OnReview()
    {
        IsLoading = true;
        IsModalVisible = false;
        CurrentStepIndex = 2;
        ActiveTabIndex = 2;
        Status = RecordRequestStates.Review.ToString();
        IsLoading = false;
    }

    protected void OnRelease()
    {
        IsLoading = true;
        IsModalVisible = false;
        CurrentStepIndex = 3;
        ActiveTabIndex = 3;
        Status = RecordRequestStates.Release.ToString();
        IsLoading = false;
    }

    protected void OnClaim()
    {
        IsLoading = true;
        IsModalVisible = false;
        Status = RecordRequestStates.Claimed.ToString();
        NavigationManager.NavigateTo("/request-management");
        IsLoading = false;
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
}
