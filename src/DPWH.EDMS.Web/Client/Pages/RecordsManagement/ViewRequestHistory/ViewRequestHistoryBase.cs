using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.ViewRequestHistory;

public class ViewRequestHistoryBase : RequestDetailsOverviewBase
{
    [Parameter] public required string Id { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    protected GetUserByIdResult SelectedUser { get; set; } = new();

    protected override void OnParametersSet()
    {
        CancelReturnUrl = "/records-management";
    }

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        await LoadUserData();

        await LoadData((res) =>
        {
            SelectedRecordRequest = res;

            BreadcrumbItems.AddRange(new List<BreadcrumbModel>
            {
                new BreadcrumbModel
                {
                    Icon = "menu",
                    Text = "Records Management",
                    Url = "/records-management",
                },
                new BreadcrumbModel
                {
                    Icon = "menu",
                    Text = "Records",
                    Url = $"/records-management/{Id}",
                },
                new BreadcrumbModel
                {
                    Icon = "create_new_folder",
                    Text = "Request History",
                    Url = NavManager.Uri.ToString()
                }
            });

            StateHasChanged();
        });

        

        IsLoading = false;
        StateHasChanged();
    }

    protected async Task LoadUserData()
    {
        await UsersService.GetById(Guid.Parse(Id));
    }
}
