using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Pages.RequestManagement.Model;
using DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequestForm;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.Services.Drawing;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.GeneratedForms.ViewTransmittalReceipt;

public class TransmittalReceiptFormBase : ComponentBase
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Parameter] public required string RequestId { get; set; }
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    [Inject] protected DrawingService drawingService { get; set; } = default!;
    protected bool IsLoading { get; set; } = false;
    protected RecordRequestModel SelectedRecordRequest { get; set; } = new();
    protected GetUserByIdResult User = new GetUserByIdResult();
    protected string UserRole { get; set; } = string.Empty;
    protected ElementReference PdfContainerRef { get; set; }
    [Parameter] public required string IsExporting { get; set; } 
    protected async override Task OnInitializedAsync()
    {

        IsLoading = true;
        await FetchUser();

        await LoadData((res) =>
        {
            SelectedRecordRequest = res;
            
        });
         
        IsLoading = false;
    }

    protected async Task LoadData(Action<RecordRequestModel> onLoadCb)
    {
        await ExceptionHandlerService.HandleApiException(async () => {
            var recordReq = await RequestManagementService.GetById(Guid.Parse(RequestId));

            if (recordReq.Success)
            {
                if (onLoadCb != null)
                {
                    onLoadCb.Invoke(recordReq.Data);
                }
            }
            else
            {
                ToastService.ShowError("Something went wrong on loading record request.");
            }
        });
    }

    protected async Task FetchUser()
    {
        var authState = await AuthenticationStateAsync!;
        var user = authState.User;
        var userId = user.GetUserId();

        var userRes = await UsersService.GetById(userId);

        if (userRes.Success)
        {
            User.UserAccess = userRes.Data.UserAccess;
            User.Office = userRes.Data.Office;
        }
        else
        {
            ToastService.ShowError("Something went wrong on fetching user data");
        }

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var roleValue = user.Claims.FirstOrDefault(c => c.Type == "role")!.Value;
            UserRole = GetRoleLabel(roleValue);
        }
    }
    protected string GetOfficeName(string officeCode)
    {
        return officeCode switch
        {
            "RMD" => "Records Management Division",
            "HRMD" => "Human Resource Management Division",
            _ => string.Empty
        };
    }
   
    protected async Task ExportPdf()
    {
        IsLoading = true;

        if (SelectedRecordRequest?.RequestedRecords is null)
            return;

        var options = new { padding = "0cm", margin = "0cm", paperSize = "A4", scale = 0.7, multiPage = true, landscape = false, };
        var data = await drawingService.ExportPdf(PdfContainerRef, options);
        await drawingService.SaveAs(data, $"Transmittal-Receipt-{DateTime.Now.ToString("MMM dd, yyyy")}.pdf");

        IsLoading = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(IsExporting))
        {
            if (IsExporting.Equals("exporting", StringComparison.OrdinalIgnoreCase))
            {
               await ExportPdf();
            }
        }
    }

    private string GetRoleLabel(string roleValue)
    {
        return ApplicationRoles.GetDisplayRoleName(roleValue, "Unknown Role");
    }
    protected string RemoveOffice(string officeValue)
    {
        if (string.IsNullOrEmpty(officeValue))
            return string.Empty;

        if (officeValue == "RMD")
        {
            return "HRMD";
        }
        else if (officeValue == "HRMD")
        {
            return "RMD";
        }

        return string.Empty;
    }
}