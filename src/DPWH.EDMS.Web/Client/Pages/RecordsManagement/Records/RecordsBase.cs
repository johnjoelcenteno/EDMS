using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordManagement;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.Records;

public class RecordsBase : GridBase<RecordRequestModel>
{
    [Parameter] public required string Id { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }  
    [Inject] public required IRecordManagementService RecordManagementService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    protected GetUserByIdResult SelectedUser { get; set; } = new();
    protected List<Document> DocumentList = new List<Document>();
    protected GetUserByIdResultBaseApiResponse EmployeeDetails { get; set; }
    protected GetLookupResultIEnumerableBaseApiResponse GetEmployeeRecords { get; set; } = new GetLookupResultIEnumerableBaseApiResponse();
    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;

        await LoadData((res) =>
        {
            SelectedUser = res;

            BreadcrumbItems.AddRange(new List<BreadcrumbModel>
            {
                new BreadcrumbModel
                {
                    Icon = "menu",
                    Text = "Records Management",
                    Url = "/records-management",
                },
                new BreadcrumbModel
                {
                    Icon = "create_new_folder",
                    Text = "Records",
                    Url = NavManager.Uri.ToString(),
                },
            });

            StateHasChanged();
        });

        await LoadRequestHistoryData();
        await GetDocumentRecords();
        IsLoading = false;
        StateHasChanged();
    }
    protected async Task GetDocumentRecords()
    {
        var res = await LookupsService.GetEmployeeRecords();
        if (res.Success)
        {
            GetEmployeeRecords = res;
        }
    }
    protected async Task LoadData(Action<GetUserByIdResult> onLoadCb)
    {
        var getRecord = await UsersService.GetById(Guid.Parse(Id));

        if (getRecord.Success)
        {
            EmployeeDetails = getRecord;
            if (onLoadCb != null)
            {
                onLoadCb.Invoke(getRecord.Data);
            }
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading user details.");
            NavManager.NavigateTo("/records-management");
        }
    }

    protected async Task LoadRequestHistoryData()
    {
        var result = await RecordManagementService.QueryByEmployeeId(SelectedUser.EmployeeId, DataSourceReq);
        if(result.Data != null)
        {
            GridData = GenericHelper.GetListByDataSource<RecordRequestModel>(result.Data);
            TotalItems = result.Total;
        }
    }

    protected void GoToSelectedItemOverview(GridRowClickEventArgs args)
    {
        IsLoading = true;

        var selectedItem = args.Item as RecordRequestModel;

        if (selectedItem != null)
        {
            NavManager.NavigateTo($"/records-management/{Id}/request-history-{selectedItem.Id.ToString()}");
        }

        IsLoading = false;
    }
    public async Task viewData(GridCommandEventArgs args)
    {
        GetLookupResult selectedId = args.Item as GetLookupResult;

        //Int32.TryParse(samp, out sampNumber);
        NavigationManager.NavigateTo($"/records-management/{EmployeeDetails.Data.EmployeeId}/{selectedId.Id}");
    }
}