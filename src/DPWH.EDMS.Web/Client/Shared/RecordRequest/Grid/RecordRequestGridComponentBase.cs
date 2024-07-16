using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.Enums;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace DPWH.EDMS.Web.Client.Shared.RecordRequest.Grid;

public class RecordRequestGridComponentBase : GridBase<EmployeeModel>
{
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }

    protected string EmployeeId { get; set; } = string.Empty;
    protected int ActiveTabIndex { get; set; } = 1;
    protected HashSet<string> RequestStates = new HashSet<string>();
    protected List<EmployeeModel> RequestRecords { get; set; } = new List<EmployeeModel>();
    protected List<EmployeeModel> AllRequestRecords { get; set; } = new List<EmployeeModel>();
    protected DateTime? SelectedDate { get; set; }

    protected string SelectedStatus = string.Empty;
    protected bool IsFilterable { get; set; } = true;

    protected List<string> StatusList = new List<string>
    {
        "Review",
        "Release",
        "Claimed"
    };

    protected async Task HandleOnLoadGrid()
    {
        LoadRequestStates();
        await GetRecordRequest();
    }

    private async Task GetRecordRequest()
    {
        var res = await RequestManagementService.Query(DataSourceReq);

        if (res.Data != null)
        {
            var getData = GenericHelper.GetListByDataSource<EmployeeModel>(res.Data);
            AllRequestRecords = getData;
            RequestRecords = new List<EmployeeModel>(AllRequestRecords);
        }

    }

    protected void HandleGoToAddNewRequest(string uri)
    {
        NavManager.NavigateTo(uri);
    }

    protected void HandleSelectedItemOverview(GridRowClickEventArgs args, string uri)
    {
        IsLoading = true;

        var selectedItem = args.Item as EmployeeModel;

        if (selectedItem != null)
        {
            NavManager.NavigateTo(uri + selectedItem.Id.ToString());
        }

        IsLoading = false;
    }

    protected void SetDateFilter(CompositeFilterDescriptor filterDescriptor)
    {
        filterDescriptor.FilterDescriptors.Clear();
        if (SelectedDate.HasValue)
        {
            var selectedDatePickerFrom = new FilterDescriptor(nameof(EmployeeModel.DateRequested), FilterOperator.IsGreaterThan, SelectedDate);
            var selectedDatePickerTo = new FilterDescriptor(nameof(EmployeeModel.DateRequested), FilterOperator.IsLessThan, SelectedDate.Value.AddDays(1));


            filterDescriptor.FilterDescriptors.Add(selectedDatePickerFrom);
            filterDescriptor.FilterDescriptors.Add(selectedDatePickerTo);
        }

        StateHasChanged();
    }

    protected void SetStatusFilter(CompositeFilterDescriptor filterDescriptor)
    {
        filterDescriptor.FilterDescriptors.Clear();

        if (!string.IsNullOrEmpty(SelectedStatus))
        {
            var selectedDatePickerFrom = new FilterDescriptor(nameof(EmployeeModel.Status), FilterOperator.IsEqualTo, SelectedStatus);

            filterDescriptor.FilterDescriptors.Add(selectedDatePickerFrom);

        }

        StateHasChanged();
    }

    protected void LoadRequestStates()
    {
        RequestStates = new HashSet<string>(
            Enum.GetValues(typeof(RecordRequestTabStates))
                .Cast<RecordRequestTabStates>()
                .Select(e => e.ToString())
        );
    }
    
    protected void TabChangedHandler(int newIndex)
    {
        IsLoading = true;
        ActiveTabIndex = newIndex;
        string status = Enum.GetName(typeof(RecordRequestTabStates), ActiveTabIndex)!;

        if (ActiveTabIndex == 0)
        {
            RequestRecords = new List<EmployeeModel>(AllRequestRecords);
            IsFilterable = true;
        }
        else
        {
            RequestRecords = AllRequestRecords.Where(x => x.Status == status).ToList();
            IsFilterable = false;
        }

        IsLoading = false;
        StateHasChanged();
    }
 
}
