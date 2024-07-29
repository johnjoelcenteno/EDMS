using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Components.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Runtime.InteropServices;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Components.Components.ReusableGrid;

public class GridBase<T> : RxBaseComponent
{
    [Inject] public required IToastService ToastService { get; set; }
    protected List<T> GridData { get; set; } = new();
    protected TelerikGrid<T> GridRef { get; set; } = new();
    protected int Page { get; set; } = 1;
    protected int PageSize { get; set; } = 5;
    protected int PageMapSize { get; set; } = 3;
    protected string GridHeight { get; set; } = "auto";
    protected List<int?> PageSizes { get; set; } = new List<int?> { 5, 10, 15 };
    protected int TotalItems { get; set; }
    protected bool IsLoading = false;
    protected string SearchValue = string.Empty;
    protected string SearchValue2 = string.Empty;
    protected string SearchValue3 = string.Empty;
    protected DataSourceRequest DataSourceReq { get; set; } = new();
    protected Filter SearchFilterRequest { get; set; } = new();
    protected Filter SearchFilterRequest2 { get; set; } = new();
    protected Filter SearchFilterRequest3 { get; set; } = new();
    protected Sort SortRequest { get; set; } = new();

    protected Func<DataSourceRequest, Task<DataSourceResult>> ServiceCb { get; set; } = default!;
    protected virtual async Task LoadData(bool bPageChanged = false)
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
            var result = await ServiceCb(DataSourceReq);

            // Convert the retrieved data to a list of BookingModel objects
            GridData = GenericHelper.GetListByDataSource<T>(result.Data);

            // Set the total number of items for pagination purposes
            TotalItems = result.Total;
        }
        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
        {
            var problemDetails = apiExtension.Result;            
            var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
            ToastService.ShowError(error);

            if (problemDetails.Status == 401)
                NavManager.NavigateTo("/401", true);
        }
        catch (Exception ex) when (ex is ApiException apiExt)
        {
            var htmlContent = new RenderFragment(builder =>
            {
                builder.AddMarkupContent(0, apiExt.Message);
            });
            ToastService.ShowError(htmlContent);

            if (apiExt.StatusCode == 401)
                NavManager.NavigateTo("/401", true);
        }

        // Loading is complete
        IsLoading = false;
    }

    //protected virtual async Task LoadMapsData(bool bPageChanged = false)
    //{
    //    IsLoading = true;

    //    // Set the number of items to retrieve
    //    DataSourceReq.Take = PageMapSize;

    //    // Calculate the number of items to skip based on the current page
    //    if (bPageChanged)
    //    {
    //        DataSourceReq.Skip = (Page - 1) * PageMapSize;
    //    }
    //    else
    //    {
    //        Page = 1;
    //        DataSourceReq.Skip = 0;
    //    }

    //    // Call the method to construct the filter requests
    //    GetFilterRequests();

    //    try
    //    {
    //        // Retrieve data from the BookingService based on the filter requests
    //        var result = await ServiceCb(DataSourceReq);

    //        // Convert the retrieved data to a list of BookingModel objects
    //        GridData = GenericHelper.GetListByDataSource<T>(result.Data);

    //        // Set the total number of items for pagination purposes
    //        TotalItems = result.Total;
    //    }
    //    catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
    //    {
    //        var problemDetails = apiExtension.Result;
    //        var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
    //        _ToastService.ShowError(error);
    //    }

    //    // Loading is complete
    //    IsLoading = false;
    //}
    protected void AddTextSearchFilterIfHasValue(List<Filter> filters, string propertyName, string value, [Optional] string fOperator)
    {
        if (!string.IsNullOrEmpty(value))
        {
            AddTextSearchFilter(filters, propertyName, value, fOperator);
        }
    }

    protected void AddTextSearchFilter(List<Filter> filters, string fieldName, string searchValue, [Optional] string fOperation)
    {
        var fOperator = string.IsNullOrEmpty(fOperation) ? DataSourceHelper.CONTAINS_OPERATOR : fOperation;

        var filter = new Filter
        {
            Field = fieldName,
            Operator = fOperator,
            Value = searchValue
        };

        filters.Add(filter);
    }

    protected virtual void GetFilterRequests(List<Filter> filters = default!)
    {
        if (filters == null || filters.Count < 1)
        {
            filters = new List<Filter>();
        }

        filters.Add(SearchFilterRequest);
        filters.Add(SearchFilterRequest2);
        filters.Add(SearchFilterRequest3);

        DataSourceReq.Filter = new Filter
        {
            Filters =
                new List<Filter>(filters)
                // Apply filtering conditions based on Field and Value not being empty or null
                .Where(f =>
                    (f.Filters != null && f.Filters.Count > 0) || !string.IsNullOrEmpty(f.Field) && !string.IsNullOrEmpty(f.Value.ToString()))
                // Make a deep copy of each filter
                .Select(f => f.DeepCopy())
                .ToList()
        };

        // Set the Logic property based on whether there are any filters or not
        DataSourceReq.Filter.Logic = DataSourceReq.Filter.Filters.Any() ? DataSourceHelper.AND_LOGIC : null;

        if (SortRequest != null && !string.IsNullOrEmpty(SortRequest.Field))
        {
            DataSourceReq.Sort = new List<Sort>() { SortRequest };
        }
        else
        {
            DataSourceReq.Sort = new List<Sort>();
        }
    }

    /*
     * OnStateChanged Function: SORTING INTEGRATION
     * - to use sorting, add the following parameters to Telerik Grid "<TelerikGrid>"
     * 1. Sortable="true"
     * 2. OnStateChanged="@OnStateChanged"
     * 3. @ref="GridRef"
     */
    protected async Task OnStateChanged(GridStateEventArgs<T> args)
    {
        var state = args.GridState;

        if (state.SortDescriptors.Count > 0)
        {
            var telerikSort = state.SortDescriptors.ToList().FirstOrDefault();
            if (telerikSort != null)
            {
                if (telerikSort.SortDirection == Telerik.DataSource.ListSortDirection.Ascending)
                {
                    SortRequest.Dir = DataSourceHelper.ASC_DIR; // Assuming ASC_DIR is the constant for ascending direction
                    SortRequest.Field = telerikSort.Member;
                }
                else if (telerikSort.SortDirection == Telerik.DataSource.ListSortDirection.Descending)
                {
                    SortRequest.Dir = DataSourceHelper.DESC_DIR; // Assuming DESC_DIR is the constant for descending direction
                    SortRequest.Field = telerikSort.Member;
                }
                else
                {
                    SortRequest = new();
                    state.SortDescriptors.Clear();
                }
            }

            await GridRef.SetStateAsync(state);
            await LoadData();
        }
    }

    protected async Task PageChangedHandler(int page)
    {
        // Update the current page
        Page = page;

        // Load data for the updated page
        await LoadData(true);
    }

    protected async Task PageSizeChangedHandler(int newPageSize)
    {
        PageSize = newPageSize;
        // Load data for the updated page
        await LoadData(true);
    }

    //protected async Task PageMapChangedHandler(int page)
    //{
    //    // Update the current page
    //    Page = page;

    //    // Load data for the updated page
    //    await LoadMapsData(true);
    //}

    //protected async Task PageMapSizeChangedHandler(int newPageSize)
    //{
    //    PageMapSize = newPageSize;
    //    // Load data for the updated page
    //    await LoadMapsData(true);
    //}
    protected virtual void UpdateHandler(GridCommandEventArgs args)
    {
        //var product = (ProductDto)args.Item;
        //product.CategoryName = Categories.FirstOrDefault(c => c.CategoryId == product.CategoryId)?.CategoryName;
        //ProductService.UpdateProduct(product);
        //LoadData();
    }

    protected virtual void DeleteHandler(GridCommandEventArgs args)
    {
        //ProductService.DeleteProduct((ProductDto)args.Item);
        //LoadData();
    }

    protected virtual void CreateHandler(GridCommandEventArgs args)
    {
        //var product = (ProductDto)args.Item;
        //product.CategoryName = Categories.FirstOrDefault(c => c.CategoryId == product.CategoryId)?.CategoryName;
        //ProductService.CreateProduct(product);
        //LoadData();
    }
}