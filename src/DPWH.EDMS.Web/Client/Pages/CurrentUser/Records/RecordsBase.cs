﻿using DocumentFormat.OpenXml.InkML;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordManagement;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.Common.Model;
using DPWH.EDMS.Web.Client.Pages.RecordsManagement.Employee.Records.Model;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Common.Trees.Models;
using Telerik.SvgIcons;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records;

public class RecordsBase : GridBase<GetLookupResult>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Parameter] public required GetUserByIdResult SelectedEmployee { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    [Inject] protected IJSRuntime? JS { get; set; }
    [Inject] public required IRecordManagementService RecordManagementService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    protected GetLookupResultIEnumerableBaseApiResponse GetEmployeeRecords { get; set; } = new GetLookupResultIEnumerableBaseApiResponse();
    protected List<RecordDocumentModel> RecordDocuments { get; set; } = new();
    protected int pageAction = 5;
    protected List<int?> PageSizesChild { get; set; } = new List<int?> { 5, 10, 15 };

    protected string DisplayName = "---";
    protected string Role = string.Empty;
    protected string EmployeeId;

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Records",
            Url = "/my-records"
        });
    }

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;

        await GetUserInfo();
        await GetDocumentRecords();
        await GetMyRecords();

        IsLoading = false;
    }
    protected async Task GetDocumentRecords()
    {
        IsLoading = true;

        var res = await LookupsService.GetPersonalRecords();
        if (res.Success)
        {
            GetEmployeeRecords = res;
        }

        IsLoading = false;
    }
    private async Task GetUserInfo()
    {
        if (AuthenticationStateAsync is null)
            return;


        var authState = await AuthenticationStateAsync;
        var user = authState.User;

        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
           
            var roleValue = user.Claims.Where(c => c.Type == "role")!.ToList();
            var role = roleValue.FirstOrDefault(role => !string.IsNullOrEmpty(role.Value) && role.Value.Contains(ApplicationRoles.RolePrefix))?.Value ?? string.Empty;

            EmployeeId = ClaimsPrincipalExtensions.GetEmployeeNumber(user)!;
            DisplayName = ClaimsPrincipalExtensions.GetDisplayName(user)!;

            Role = GetRoleLabel(role);

        }
    }
    private string GetRoleLabel(string roleValue)
    {
        return ApplicationRoles.GetDisplayRoleName(roleValue, "Unknown Role");
    }
    protected async Task GetMyRecords()
    {
        var recordResult = await RecordManagementService.QueryByEmployeeId(EmployeeId, DataSourceReq);
        if (recordResult.Data != null)
        {
            RecordDocuments = GenericHelper.GetListByDataSource<RecordDocumentModel>(recordResult.Data);
        }
    }
    //protected void viewData(GetLookupResult data)
    //{
    //    NavigationManager.NavigateTo($"/my-records/{data.Id}");
    //}
    protected async Task DownloadFromStream(string uri, string name)
    {
        var fileUri = uri;
        var fileStream = await GetFileStreamFromUri(fileUri);
        var fileName = name;

        using var streamRef = new DotNetStreamReference(stream: fileStream);
        await JS!.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }
    protected async Task<Stream> GetFileStreamFromUri(string fileUri)
    {
        using var httpClient = new HttpClient();
        var fileBytes = await httpClient.GetByteArrayAsync(fileUri);
        var fileStream = new MemoryStream(fileBytes);
        return fileStream;
    }
    //protected void GoToAddNewRequest()
    //{
    //    HandleGoToAddNewRequest("record-management/request-history/add/" + SelectedEmployee.Id);
    //}
    protected void GoToAddNewRecord()
    {
        // NavigationManager.NavigateTo("record-management/records/add/" + SelectedEmployee.Id);
    }
    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        //HandleSelectedItemOverview(args, "request-management/view-request-form/");
    }
}