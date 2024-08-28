using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandlerPIS;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.Employee.RequestHistory;

public class RequestFormBase : RequestFormComponentBase
{
    [Parameter] public required string UserId { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }

    protected override void OnInitialized()
    {
        HandleOnInit();
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await GetEmployeeDetails();
        await HandleLoadItems();
        IsLoading = false;
    }

    #region Load Events   
    private async Task GetEmployeeDetails()
    {
        var isSuccess = await ExceptionHandlerService.IsSuccess(async () =>
        {
            var res = await UsersService.GetById(Guid.Parse(UserId));

            if (res.Success)
            {
                SelectedItem.EmployeeNumber = res.Data.EmployeeId;
                SelectedItem.Email = res.Data.Email;
                SelectedItem.FullName = GetUserFullname(res.Data.LastName, res.Data.FirstName, res.Data.MiddleInitial);
            }
        });

        if (!isSuccess)
        {
            NavManager.NavigateTo("/records-management");
        }
    }

    private string GetUserFullname(string lastname, string firstname, string middleinitial)
    {
        if (!string.IsNullOrEmpty(lastname) || !string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(middleinitial))
        {
            var name = $"{GenericHelper.GetDisplayValue(lastname, " ")}, {firstname} {middleinitial}";
            return name;
        }
        return string.Empty;
    }
    #endregion
}
