using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.UserManagement.EditUser;

public class EditUserBase : RxBaseComponent
{
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required IToastService _ToastService { get; set; }
    [Parameter] public string Id { get; set; }
    protected Guid UserId = new Guid();

    protected override async Task OnParametersSetAsync()
    {
        IsLoading = true;
      
            if (Guid.TryParse($"{Id}", out var id))
            {
                UserId = id;
            }

            BreadcrumbItems.Add(new BreadcrumbModel
            {
                Icon = "group",
                Text = "User Management",
                Url = "/user-management"
            });

            BreadcrumbItems.Add(new BreadcrumbModel
            {
                Icon = "search",
                Text = "Edit User",
                Url = NavManager.Uri.ToString(),
            });
        
        IsLoading = false;
    }
}
