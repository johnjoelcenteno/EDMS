using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.PendingRequests.AddRequest;

public class AddRequestBase: RxBaseComponent
{
    [Inject] public required NavigationManager NavManager { get; set; }
    protected DocumentRequestModel SelectedItem { get; set; } = new();
    
    protected List<DocumentClaimant> DocumentClaimants = Enum.GetValues(typeof(DocumentClaimant)).Cast<DocumentClaimant>().ToList();
    protected List<string> AllRecords = new List<string> { "Record 1", "Record 2", "Record 3" };
    protected List<string> ValidIdTypes = new List<string> { "ID Type 1", "ID Type 2", "ID Type 3" };
    protected List<string> SupportingDocumentTypes = new List<string> { "Document Type 1", "Document Type 2", "Document Type 3" };

    protected FluentValidationValidator? FluentValidationValidator;

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Add New Request",
            Url = NavManager.Uri.ToString(),
        });
    }

    protected void OnCancel()
    {
        NavManager.NavigateTo("/my-pending-request");
    }

    protected async Task HandleSubmit(DocumentRequestModel currentDocRequest)
    {
       if(currentDocRequest != null)
        {
            SelectedItem = currentDocRequest;

            // do api call here
        }
    }

}
