using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequestForm;

public class ViewRequestFormBase : RxBaseComponent
{
    [Parameter] public required string Id { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    protected int ActiveTabIndex { get; set; } = 0;
    protected int CurrentStepIndex { get; set; } = 0;
    protected DateTime? Value { get; set; } = DateTime.Now;
    protected DateTime? SelectedTime { get; set; } = DateTime.Now;
    protected bool IsReviewed { get; set; } = false;
    protected bool IsReleased { get; set; } = false;
    protected bool IsClaimed { get; set; } = false;
    protected bool IsModalVisible { get; set; } = false;

    protected override void OnInitialized()
    {
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
    }

    protected void OnCancel()
    {
        NavigationManager.NavigateTo("/request-management");
    }

    protected void OnReview()
    {
        IsLoading = true;
        IsModalVisible = false;
        CurrentStepIndex = 1;
        ActiveTabIndex = 2;
        IsReviewed = true;
        IsLoading = false;
    }

    protected void OnRelease()
    {
        IsLoading = true;
        IsModalVisible = false;
        CurrentStepIndex = 2;
        ActiveTabIndex = 3;
        IsReleased = true;
        IsLoading = false;
    }

    protected void OnClaim()
    {
        IsLoading = true;
        IsModalVisible = false;
        CurrentStepIndex = 3;
        IsClaimed = true;
        NavigationManager.NavigateTo("/request-management");
        IsLoading = false;
    }
}
