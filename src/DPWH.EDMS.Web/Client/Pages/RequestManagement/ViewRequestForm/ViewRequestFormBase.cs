using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequestForm;

public class ViewRequestFormBase : RequestDetailsOverviewBase
{
    [Inject] public required NavigationManager NavigationManager { get; set; }
    protected int ActiveTabIndex { get; set; } = 0;
    protected int CurrentStepIndex { get; set; }
    protected string Status { get; set; } = "Submitted";
    protected DateTime? DateReceived { get; set; } = DateTime.Now;
    protected DateTime? TimeReceived { get; set; } = DateTime.Now;
    protected bool IsModalVisible { get; set; }
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
        CurrentStepIndex = 1;
        ActiveTabIndex = 2;
        Status = RecordRequestStates.Review.ToString();
        IsLoading = false;
    }

    protected void OnRelease()
    {
        IsLoading = true;
        IsModalVisible = false;
        CurrentStepIndex = 2;
        ActiveTabIndex = 3;
        Status = RecordRequestStates.Release.ToString();
        IsLoading = false;
    }

    protected void OnClaim()
    {
        IsLoading = true;
        IsModalVisible = false;
        CurrentStepIndex = 3;
        Status = RecordRequestStates.Claimed.ToString();
        NavigationManager.NavigateTo("/request-management");
        IsLoading = false;
    }
}
