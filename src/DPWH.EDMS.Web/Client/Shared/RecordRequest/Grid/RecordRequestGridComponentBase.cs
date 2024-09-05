using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.Enums;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Shared.Enums;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Shared.RecordRequest.Grid;

public class RecordRequestGridComponentBase : GridBase<RecordRequestModel>
{
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }

    protected string EmployeeId { get; set; } = string.Empty;
    protected int ActiveTabIndex { get; set; } = 1;
    protected HashSet<string> RequestStates = new HashSet<string>();

    protected async Task HandleOnLoadGrid()
    {
        LoadRequestStates();
        ServiceCb = RequestManagementService.Query;
        await LoadData();
    }

    protected void HandleGoToAddNewRequest(string uri)
    {
        NavManager.NavigateTo(uri);
    }

    protected void HandleSelectedItemOverview(GridRowClickEventArgs args, string uri)
    {
        IsLoading = true;

        var selectedItem = args.Item as RecordRequestModel;

        if (selectedItem != null)
        {
            NavManager.NavigateTo(uri + selectedItem.Id.ToString());
        }

        IsLoading = false;
    }
    protected void LoadRequestStates()
    {
        RequestStates = new HashSet<string>(
            Enum.GetValues(typeof(RecordRequestTabStates))
                .Cast<RecordRequestTabStates>()
                .Select(e => e.ToString())
        );
    }
    protected async Task TabChangedHandler(int newIndex)
    {
        ActiveTabIndex = newIndex;
        var filters = new List<Api.Contracts.Filter>();
        string status = Enum.GetName(typeof(RecordRequestTabStates), ActiveTabIndex)!;

        if (ActiveTabIndex != 0 && ActiveTabIndex != 6 && !string.IsNullOrEmpty(status))
        {
            AddTextSearchFilter(filters, nameof(RecordRequestModel.Status), status);
        }
        else if (ActiveTabIndex == 6)
        {
            AddTextSearchFilter(filters, "RMDRequestStatus", OfficeRequestedRecordStatus.Reviewed.ToString());
        }

        // Set the filters
        SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
        SearchFilterRequest.Filters = filters;

        // Load data with the updated filters
        await LoadData();
        StateHasChanged();
    }

    protected override async Task LoadData(bool bPageChanged = false)
    {
        IsLoading = true;

        // Set the number of items to retrieve
        DataSourceReq.Take = PageSize;

        // Calculate the number of items to skip based on the current page
        if (bPageChanged)
        {
            DataSourceReq.Skip = (Page - 1) * PageSize;
        }
        else
        {
            Page = 1;
            DataSourceReq.Skip = 0;
        }

        // Call the method to construct the filter requests
        GetFilterRequests();

        try
        {
            var result = new DataSourceResult();

            // Retrieve data from the BookingService based on the filter requests
            if (!string.IsNullOrEmpty(EmployeeId))
            {
                result = await RequestManagementService.QueryByEmployeeId(EmployeeId, DataSourceReq);
            }
            else
            {
                result = await RequestManagementService.Query(DataSourceReq);
            }

            // Convert the retrieved data to a list of BookingModel objects
            GridData = GenericHelper.GetListByDataSource<RecordRequestModel>(result.Data);

            // Set the total number of items for pagination purposes
            TotalItems = result.Total;
        }
        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
        {
            var problemDetails = apiExtension.Result;
            var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
            ToastService.ShowError(error);

            if (problemDetails.Status == 401)
                NavManager.NavigateTo("/logout", true);
        }
        catch (Exception ex) when (ex is ApiException apiExt)
        {
            var htmlContent = new RenderFragment(builder =>
            {
                builder.AddMarkupContent(0, apiExt.Message);
            });
            ToastService.ShowError(htmlContent);

            if (apiExt.StatusCode == 401)
                NavManager.NavigateTo("/logout", true);
        }

        // Loading is complete
        IsLoading = false;
    }
}
