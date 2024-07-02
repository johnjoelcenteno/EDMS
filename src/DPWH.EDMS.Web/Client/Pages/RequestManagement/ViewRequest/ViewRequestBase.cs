using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequest;

public class ViewRequestBase : RequestDetailsOverviewBase
{
    protected bool? IsRequestApproved;

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
                    Text = GenericHelper.GetDisplayValue(SelectedRecordRequest.ControlNumber.ToString()),
                    Url = NavManager.Uri.ToString(),
                },
            });
            
        });

        IsLoading = false;
        StateHasChanged();
    }
}