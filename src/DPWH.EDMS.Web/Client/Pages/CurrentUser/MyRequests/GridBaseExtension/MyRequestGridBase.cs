using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.MyRequests.GridBaseExtension;

public class MyRequestGridBase : GridBase<RecordRequestModel>
{
    [Inject] public required IRecordRequestsService RecordRequestsService { get; set; }

    protected string EmployeeId { get; set; } = string.Empty;

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
            // Retrieve data from the BookingService based on the filter requests
            var result = await RecordRequestsService.QueryByEmployeeId(EmployeeId, DataSourceReq);

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
