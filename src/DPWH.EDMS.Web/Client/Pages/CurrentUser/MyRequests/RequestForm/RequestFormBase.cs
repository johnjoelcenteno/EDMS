using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests.RequestForm;

public class RequestFormBase : RequestFormComponentBase
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }

    protected override void OnInitialized()
    {
        HandleOnInit();
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await _GetUser();
        await HandleLoadItems();
        IsLoading = false;
    }

    #region Load Events   
    private async Task _GetUser()
    {
        if (AuthenticationStateAsync is null)
            return;

        var authState = await AuthenticationStateAsync;
        var user = authState.User;
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var userId = user.Claims.FirstOrDefault(c => c.Type == "sub")!.Value;
            var roleValue = user.Claims.FirstOrDefault(c => c.Type == "role")!.Value;
            var userRes = await UserService.GetById(Guid.Parse(userId));
            if (userRes.Success)
            {
                CurrentUser = Mapper.Map<UserModel>(userRes.Data);
                SelectedItem.EmployeeNumber = CurrentUser.EmployeeId;
                UserFullname = _GetUserFullname();
            }
        }
    }
    private string _GetUserFullname()
    {   
        if(!string.IsNullOrEmpty(CurrentUser.LastName) || !string.IsNullOrEmpty(CurrentUser.FirstName) || !string.IsNullOrEmpty(CurrentUser.MiddleInitial))
        {
            var name = $"{GenericHelper.GetDisplayValue(CurrentUser.LastName, " ")}, {CurrentUser.FirstName} {CurrentUser.MiddleInitial}";
            return name;
        }
        return string.Empty;
    }
    #endregion
}
