using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.UserManagement.AddUser;

public class AddUserBase : RxBaseComponent
{
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Add New User",
            Url = NavManager.Uri.ToString(),
        });
    } 
    protected void OnCancel()
    {
        NavManager.NavigateTo("/user-management");
    } 
    protected async Task HandleSubmit()
    {

    }
}

