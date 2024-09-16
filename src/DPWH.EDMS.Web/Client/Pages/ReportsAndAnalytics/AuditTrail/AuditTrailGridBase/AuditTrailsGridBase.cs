using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.ReportsAndAnalytics.AuditTrail.AuditTrailGridBase;

public class AuditTrailsGridBase<T> : ComponentBase
{
    [Inject] private IToastService _ToastService { get; set; } = default!;
    protected List<T> GridData { get; set; } = new();
    protected int Page { get; set; } = 1;
    protected int PageSize { get; set; } = 5;
    protected int PageMapSize { get; set; } = 3;
    protected readonly string GridHeight = "auto";
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
    protected List<AuditLogModel> GridDataAudit { get; set; } = new();
    protected string SelectedType { get; set; }
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
            var data = GenericHelper.GetListByDataSource<AuditLogModel>(result.Data);
            
            switch (SelectedType)
            {
              
                case "Data Library":
                    GridDataAudit = data.Where(x => x.Entity == "Signatory").ToList();
                    break;
                case "Request Management":
                    GridDataAudit = data.Where(x => x.Entity == "RecordRequest").ToList();
                    break;
                case "DPWH Issuances":
                    GridDataAudit = data.Where(x => x.Entity == "RecordType").ToList();
                    break;
                case "Employee Documents":
                    GridDataAudit = data.Where(x => x.Entity == "RecordType").ToList();
                    break;
                default:
                  
                    break;
            }
            if (data != null && !string.IsNullOrEmpty(SelectedType))
            {
                GridDataAudit = data.Where(x => x.Entity == "Signatory").ToList();
            }
            else
            {
                GridData = GenericHelper.GetListByDataSource<T>(result.Data);
            }
            // Convert the retrieved data to a list of BookingModel objects
             

            // Set the total number of items for pagination purposes
            TotalItems = result.Total;
        }
        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
        {
            var problemDetails = apiExtension.Result;
            var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
            _ToastService.ShowError(error);

        }
        catch (Exception ex) when (ex is ApiException apiExtention)
        {
            var htmlContent = new RenderFragment(builder =>
            {
                builder.AddMarkupContent(0, apiExtention.Message);
            });
            _ToastService.ShowError(htmlContent);
        }

        // Loading is complete
        IsLoading = false;
    }
    protected void AddTextSearchFilterIfHasValue(List<Filter> filters, string propertyName, string value, [Optional] string fOperator)
    {
        if (!string.IsNullOrEmpty(value))
        {
            AddTextSearchFilter(filters, propertyName, value, fOperator);
        }
    }

    protected void AddTextSearchFilter(List<Filter> filters, string fieldName, string searchValue, [Optional] string fOperation)
    {
        var fOperator = string.IsNullOrEmpty(fOperation) ? DataSourceHelper.EQUAL_OPERATOR : fOperation;

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

        // Create a new list to store the rearranged filters
        var rearrangedFilters = new List<Filter>();

        // Add the SearchFilterRequest, SearchFilterRequest2, and SearchFilterRequest3 as in the original method
        rearrangedFilters.Add(SearchFilterRequest);

        // Map existing filters to the desired format
        var formattedFilters = rearrangedFilters
            .SelectMany(f => f.Filters ?? new List<Filter> { f })
            .Select(f => f.DeepCopy())
            .ToList();

        // Apply the rearranged filters to DataSourceReq.Filter
        DataSourceReq.Filter = new Filter
        {
            Value = SearchValue,
            Filters = formattedFilters
        };
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