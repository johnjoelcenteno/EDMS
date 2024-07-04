using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.AddRequest;

public class AddRequestBase : AddRequestComponentBase
{
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Add New Request",
            Url = NavManager.Uri.ToString(),
        });

        RedirectUri = "/request-management";
    }

    protected void OnCancel()
    {
        HandleCancel("/request-management");
    }   

}
