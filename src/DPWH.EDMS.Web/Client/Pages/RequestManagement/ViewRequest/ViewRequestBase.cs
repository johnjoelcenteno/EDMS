using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ViewRequest;

public class ViewRequestBase : RxBaseComponent
{
    [Parameter] public required string RequestId { get; set; }
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }

    protected RecordRequestModel SelectedRecordRequest { get; set; } = new();
    private Dictionary<Guid, string> IdTypesLookup = new Dictionary<Guid, string>();
    private Dictionary<Guid, string> AuthDocLookup = new Dictionary<Guid, string>();
    protected bool? IsRequestApproved;

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;

        var getValidIdTypes = await LookupsService.GetValidIdTypes();
        var getAuthDocTypes = await LookupsService.GetAuthorizationDocumentTypes();
        if (getValidIdTypes?.Data != null || getAuthDocTypes?.Data != null)
        {
            IdTypesLookup = getValidIdTypes?.Data.ToDictionary(c => c.Id, c => c.Name)!;
            AuthDocLookup = getAuthDocTypes?.Data.ToDictionary(d => d.Id, d => d.Name)!;
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
                    Text = "Request Management",
                    Url = "/request-management",
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
            NavManager.NavigateTo("/request-management");
        }

        IsLoading = false;
    }

    protected string GetDocumentTypeName(Guid documentTypeId)
    {
        if (IdTypesLookup.TryGetValue(documentTypeId, out var name))
        {
            return name;
        }
        else if(AuthDocLookup.TryGetValue(documentTypeId, out name))
        {
            return name;
        }
        
        return "Unknown";
    }
    protected void OnCancel()
    {
        NavManager.NavigateTo("/request-management");
    }
    protected string checkIdType(string docType)
    {
        if(docType == "Driver License" || docType == "ePassport" || docType == "Firearms License" 
            || docType == "GOCC" || docType == "GSIS")
        {
            docType = "Valid Id:";
            return docType;
        }
        else if(docType == "Authorize Letter" || docType == "Special Power of Attorney")
        {
            docType = "Authorization Document:";
            return docType;
        }
        return "";
    }
}
