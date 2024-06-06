using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using DPWH.EDMS.IDP.Core.Constants;
using AutoMapper;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.PendingRequests;

public class PendingRequestsBase : GridBase<DocumentRequestModel>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    protected bool IsEndUser = false;
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Pending Request",
            Url = NavManager.Uri.ToString(),
        });

        GridData = GetMockData();
    }

    protected async override Task OnInitializedAsync()
    {
        IsEndUser = await CheckIfEndUser();
    }

    protected async Task<bool> CheckIfEndUser()
    {

        var authState = await AuthenticationStateAsync!;
        var user = authState.User;


        return (user.Identity is not null && user.Identity.IsAuthenticated) && user.IsInRole(ApplicationRoles.EndUser);
    }

    private List<DocumentRequestModel> GetMockData()
    {
        return new List<DocumentRequestModel>
            {
                new DocumentRequestModel
                {
                    ControlNumber = "CN001",
                    RecordsRequested = new List<string> { "Record1", "Record2" },
                    DateRequested = new DateTime(2024, 6, 1),
                    Purpose = "Audit",
                    Status = "Pending"
                },
                new DocumentRequestModel
                {
                    ControlNumber = "CN002",
                    RecordsRequested = new List<string> { },
                    DateRequested = new DateTime(2024, 6, 2),
                    Purpose = "Compliance",
                    Status = "Approved"
                },
                new DocumentRequestModel
                {
                    ControlNumber = "CN003",
                    RecordsRequested = new List<string> { "Record5", "Record6", "Record3", "Record4" },
                    DateRequested = new DateTime(2024, 6, 3),
                    Purpose = "Research",
                    Status = "Rejected"
                },
                new DocumentRequestModel
                {
                    ControlNumber = "CN004",
                    RecordsRequested = new List<string> { "Record7", "Record8" },
                    DateRequested = new DateTime(2024, 6, 4),
                    Purpose = "Legal",
                    Status = "Pending"
                },
                new DocumentRequestModel
                {
                    ControlNumber = "CN005",
                    RecordsRequested = new List<string> { "Record9", "Record10" },
                    DateRequested = new DateTime(2024, 6, 5),
                    Purpose = "Review",
                    Status = "Approved"
                }                
            };
    }
}
