using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components.Breadcrumb;
using static System.Net.WebRequestMethods;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.PendingRequests.ViewRequest;

public class ViewRequestBase : RxBaseComponent
{
    [Parameter] public required string RequestId { get; set; }
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }

    protected RecordRequestModel SelectedRecordRequest { get; set; } = new();
    protected Dictionary<Guid, string> IdTypesLookup = new Dictionary<Guid, string>();


    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;

        var getValidIdTypes = await LookupsService.GetValidIdTypes();
        var getAuthDocTypes = await LookupsService.GetAuthorizationDocumentTypes();

        if (getValidIdTypes?.Data != null || getAuthDocTypes?.Data != null)
        {
            IdTypesLookup = getValidIdTypes?.Data.ToDictionary(c => c.Id, c => c.Name) ?? getAuthDocTypes.Data.ToDictionary(d => d.Id, d => d.Name);
        }

        var recordReq = await RecordRequestsService.GetById(Guid.Parse(RequestId));

        if (recordReq.Success)
        {
            SelectedRecordRequest = recordReq.Data;

            BreadcrumbItems.AddRange(new List<BreadcrumbModel>
            {
                new BreadcrumbModel
                {
                    Icon = "menu",
                    Text = "My Pending Request",
                    Url = "/my-pending-request",
                },
                new BreadcrumbModel
                {
                    Icon = "create_new_folder",
                    Text = GenericHelper.GetDisplayValue(SelectedRecordRequest.ControlNumber.ToString()),
                    Url = NavManager.Uri.ToString(),
                },
            });
        }       
        else
        {
            ToastService.ShowError("Something went wrong on loading record request.");
            NavManager.NavigateTo("/my-pending-request");
        }

        IsLoading = false;
    }

    protected string GetDocumentTypeName(Guid documentTypeId)
    {
        if (IdTypesLookup.TryGetValue(documentTypeId, out var name))
        {
            return name;
        }
        return "Unknown";
    }
}
