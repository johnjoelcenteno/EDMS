using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.View.RequestDetailsOverview;
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Breadcrumb;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequestedRecordForm;

public class ViewRequestedRecordFormBase : ComponentBase
{
    [Parameter] public required string RequestId { get; set; }
    [Parameter] public required string DocumentId { get; set; }
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    protected RecordRequestModel SelectedRecordRequest { get; set; } = new();
    protected RequestedRecordModel RequestedRecord { get; set; } = new();
    [Inject] public required NavigationManager NavManager { get; set; }

    protected string CancelReturnUrl = string.Empty;
    protected bool IsLoading { get; set; }
    public List<BreadcrumbModel> BreadcrumbItems { get; set; } = new()
    {
         new() { Icon = "home", Url = "/"},
    };
    protected bool XSmall { get; set; }
    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        await LoadData(async (res) =>
        {
            SelectedRecordRequest = res;
            if(SelectedRecordRequest != null)
            {
                LoadDocument(SelectedRecordRequest);
            }
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
                    Url = $"/request-management/view-request-form/{RequestId}",
                },
                new BreadcrumbModel
                {
                    Icon = "create_new_folder",
                    Text = $"{RequestedRecord.RecordType}",
                    Url = NavManager.Uri.ToString(),
                },
            });
        });
        IsLoading = false;
    }

    protected void LoadDocument(RecordRequestModel data)
    {
        var guid = Guid.Parse(DocumentId);
        var document = data.RequestedRecords.FirstOrDefault(x => x.Id == guid);
        if (document != null) {
            RequestedRecord = document;
        }
        if (string.IsNullOrEmpty(RequestedRecord.Uri)) {
            NavManager.NavigateTo("/404");
        }
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
                NavManager.NavigateTo(CancelReturnUrl);
            }
        });
    }
}