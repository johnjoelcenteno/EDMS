using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.Employee;

public class EmployeeBase : RxBaseComponent
{
    [Parameter] public required string UserId { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    protected GetUserByIdResult SelectedEmployee { get; set; } = new GetUserByIdResult();
    protected GetLookupResultIEnumerableBaseApiResponse GetEmployeeRecords { get; set; } = new GetLookupResultIEnumerableBaseApiResponse();

    protected override void OnInitialized()
    {
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
                    Icon = "create_new_folder",
                    Text = "Records",
                    Url = NavManager.Uri.ToString(),
                },
            });
    }

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        await LoadSelectedEmployeeData();
        IsLoading = false;
    }
    protected async Task LoadSelectedEmployeeData()
    {
        var selectedEmp = await UsersService.GetById(Guid.Parse(UserId));

        if (selectedEmp.Success)
        {
            SelectedEmployee = selectedEmp.Data;
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading user details.");
            NavManager.NavigateTo("/records-management");
        }
    }
}
