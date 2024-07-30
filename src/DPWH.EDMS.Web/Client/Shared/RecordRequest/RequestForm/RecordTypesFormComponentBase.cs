using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordTypes;
using DPWH.EDMS.Client.Shared.Configurations;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.Common.Model;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;

public class RecordTypesFormComponentBase : GridBase<RecordsLibraryModel>
{
    #region Dependency Injection
    [Inject] public required IRecordTypesService RecordTypesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }


    [Inject] public required IDataLibraryService DataLibraryService { get; set; }
    [Inject] public required ConfigManager ConfigManager { get; set; }
    #endregion


    #region Parameter
    [Parameter] public EventCallback<RecordsLibraryModel> HandleCreateOnSubmit { get; set; }
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    [Parameter] public string Type { get; set; }
    [Parameter] public RecordsLibraryModel EditItem { get; set; }
    [Parameter] public string DataType { get; set; }

    #endregion

    protected RecordsLibraryModel NewConfig { get; set; } = new RecordsLibraryModel();

    protected FluentValidationValidator? FluentValidationValidator { get; set; }

    protected List<string> SectionList { get; set; } = new List<string>();
    protected List<string> OfficeList { get; set; } = new List<string>();
    protected bool IsSectionEmpty { get; set; } = false;
    protected bool IsOfficeEmpty { get; set; } = false;
    protected string SelectedSection = string.Empty;
    protected string SelectedOffice = string.Empty;
    protected string SelectedRecordType = string.Empty;

    protected string? SearchName { get; set; }
    protected string? SearchCategory { get; set; }
    protected string? SearchCode { get; set; }
    protected string? SearchSection { get; set; }
    protected string? SearchOffice { get; set; }
    protected string? SearchCreatedBy { get; set; }
    protected DateTime? SelectedCreated { get; set; }

    protected TelerikDialog DialogReference = new();
    #region Submit Events
    protected async Task HandleOnSubmitCallback()
    {
        if (await FluentValidationValidator!.ValidateAsync())
        {
            await HandleCreateOnSubmit.InvokeAsync(NewConfig);
        }
    }

    protected async Task HandleOnCancelCallBack()
    {
        if (HandleOnCancel.HasDelegate)
        {
            await HandleOnCancel.InvokeAsync();
        }
    }
    #endregion

    #region Load Events
    protected async Task LoadSection()
    {
        SectionList = ConfigManager.SectionDataLibrary.ToList();
    }

    protected async Task LoadOffice()
    {
        OfficeList = ConfigManager.OfficeDataLibrary.ToList();
    }
    protected virtual async Task LoadLibraryData()
    {
        IsLoading = true;

        await ExceptionHandlerService.HandleApiException(async () =>
        {
            ServiceCb = RecordTypesService.QueryRecordTypesAsync;

            var filters = new List<Api.Contracts.Filter>();
            AddTextSearchFilterIfNotNull(filters, nameof(RecordsLibraryModel.Category), DataType, "eq");
            AddTextSearchFilterIfNotNull(filters, nameof(RecordsLibraryModel.IsActive), true.ToString(), "eq");
            SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
            SearchFilterRequest.Filters = filters.Any() ? filters : null;
            await LoadData();
        });
        IsLoading = false;
    }
    protected async Task LoadCurrentValues()
    {
        if (EditItem.Name != null && Type == "Edit")
        {
            NewConfig.Name = EditItem.Name;
            NewConfig.Id = EditItem.Id;
            NewConfig.Category = DataType;
            NewConfig.Section = EditItem.Section;
            NewConfig.Office = EditItem.Office;
            NewConfig.Code = EditItem.Code;
        }
        else
        {
            NewConfig.Category = DataType;
        }



    }
    #endregion

    #region OnChange Events
    protected async Task SectionDropdownErrorChecker()
    {
        IsSectionEmpty = string.IsNullOrEmpty(NewConfig.Section);
    }
    protected async Task OfficeDropdownErrorChecker()
    {
        IsOfficeEmpty = string.IsNullOrEmpty(NewConfig.Office);
    }
    #endregion

    protected async void SetFilterGrid()
    {
        var filters = new List<Api.Contracts.Filter>();
        AddDateFilter(filters);
        AddTextSearchFilterIfNotNull(filters, nameof(RecordsLibraryModel.Name), SearchName?.ToString(), "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordsLibraryModel.Category), DataType, "eq");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordsLibraryModel.Code), SearchCode, "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordsLibraryModel.Section), SearchSection, "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordsLibraryModel.Office), SearchOffice, "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordsLibraryModel.CreatedBy), SearchCreatedBy, "contains");
        AddTextSearchFilterIfNotNull(filters, nameof(RecordsLibraryModel.IsActive), true.ToString(), "eq");

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
            AddTextSearchFilter(filters, nameof(RecordsLibraryModel.Created), SelectedCreated.Value.ToString(), "gte");
            AddTextSearchFilter(filters, nameof(RecordsLibraryModel.Created), SelectedCreated.Value.AddDays(1).ToString(), "lte");
        }
    }
    protected void SetOfficeFilter(CompositeFilterDescriptor filterDescriptor)
    {
        filterDescriptor.FilterDescriptors.Clear();

        if (!string.IsNullOrEmpty(SelectedOffice))
        {
            var selectedFrom = new FilterDescriptor(SelectedRecordType, FilterOperator.IsEqualTo, SelectedOffice);

            filterDescriptor.FilterDescriptors.Add(selectedFrom);

        }

        StateHasChanged();
    }
    protected void SetSectionFilter(CompositeFilterDescriptor filterDescriptor)
    {
        filterDescriptor.FilterDescriptors.Clear();

        if (!string.IsNullOrEmpty(SelectedSection))
        {
            var selectedFrom = new FilterDescriptor(SelectedRecordType, FilterOperator.IsEqualTo, SelectedSection);

            filterDescriptor.FilterDescriptors.Add(selectedFrom);

        }

        StateHasChanged();
    }
}
