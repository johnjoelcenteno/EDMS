using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.Common.Enum;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Telerik.Blazor.Components;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.Common.Model;

namespace DPWH.EDMS.Web.Client.Shared.DataLibrary.RequestForm;

public class DataLibraryRequestFormComponentBase : GridBase<DataManagementModel>
{
    [Inject] public required IDataLibraryService DataLibraryService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Parameter] public EventCallback<ConfigModel> HandleCreateOnSubmit { get; set; }
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    [Parameter] public string DataType { get; set; }
    [Parameter] public string Type { get; set; }

    [Parameter] public ConfigModel EditItem { get; set; } = default!;
    protected ConfigModel NewConfig { get; set; } = new ConfigModel();
    protected TelerikDialog dialogReference = new();
    //validator
    protected FluentValidationValidator? FluentValidationValidator;
    protected bool IsVisible {  get; set; } = false;
    protected string? SearchValue { get; set; }
    protected string? SearchCreatedBy { get; set; }
    protected DateTime? SelectedCreated { get; set; }

    #region Load Events

    protected virtual async Task LoadLibraryData()
    {
        IsLoading = true;

        await ExceptionHandlerService.HandleApiException(async () =>
        {
            ServiceCb = DataLibraryService.GetDataLibraries;

            var filters = new List<Api.Contracts.Filter>();
            AddTextSearchFilterIfNotNull(filters, nameof(DataManagementModel.Type), DataType, "eq");
            AddTextSearchFilterIfNotNull(filters, nameof(DataManagementModel.IsDeleted), false.ToString(), "eq");
            SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
            SearchFilterRequest.Filters = filters.Any() ? filters : null;
            await LoadData();

        });

        IsLoading = false;
    }
    protected async void SetFilterGrid()
    {
        var filters = new List<Api.Contracts.Filter>();
        AddDateFilter(filters);
        AddTextSearchFilterIfNotNull(filters, nameof(DataManagementModel.Value), SearchValue?.ToString(), "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(DataManagementModel.Type), DataType, "eq");
        AddTextSearchFilterIfNotNull(filters, nameof(DataManagementModel.CreatedBy), SearchCreatedBy, "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(DataManagementModel.IsDeleted), false.ToString(), "eq");
        Console.WriteLine(DataType);
        SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
        SearchFilterRequest.Filters = filters.Any() ? filters : null;

        await LoadData();
        StateHasChanged();
    }

    private void AddTextSearchFilterIfNotNull(List<Api.Contracts.Filter> filters, string fieldName, string? value, string operation)
    {
        if (!string.IsNullOrEmpty(value))
        {
            AddTextSearchFilter(filters, fieldName, value, operation);
        }
    }
    private void AddDateFilter(List<Api.Contracts.Filter> filters)
    {
        if (SelectedCreated.HasValue)
        {
            AddTextSearchFilter(filters, nameof(DataManagementModel.Created), SelectedCreated.Value.ToString(), "gte");
            AddTextSearchFilter(filters, nameof(DataManagementModel.Created), SelectedCreated.Value.AddDays(1).ToString(), "lte");
        }
    }
    #endregion

    #region Submit Events
    protected async Task HandleOnSubmitCallback()
    {
        if (await FluentValidationValidator!.ValidateAsync())
        {
            if (HandleCreateOnSubmit.HasDelegate)
            {
                await HandleCreateOnSubmit.InvokeAsync(NewConfig);
            }
        }
    }
    protected void HandleCancel(string uri)
    {
        NavManager.NavigateTo(uri);
    }
    protected async Task HandleOnCancelCallBack()
    {
        if (HandleOnCancel.HasDelegate)
        {
            await HandleOnCancel.InvokeAsync();
        }
    }
    #endregion

    //#region Optional Validation
    //protected bool IsEmployee()
    //{
    //}
    //#endregion
}

