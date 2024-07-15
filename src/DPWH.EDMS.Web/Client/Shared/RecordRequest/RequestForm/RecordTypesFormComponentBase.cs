using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordTypes;
using DPWH.EDMS.Client.Shared.Configurations;
using DPWH.EDMS.Components;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.Common.Model;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;

public class RecordTypesFormComponentBase : RxBaseComponent 
{
    #region Dependency Injection
    [Inject] public required IRecordTypesService RecordTypesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }


    [Inject] public required IDataLibraryService DataLibraryService { get; set; }
    [Inject] public required ConfigManager ConfigManager { get; set; }
    #endregion


    #region Parameter
    [Parameter] public EventCallback<RecordsLibraryModel> HandleCreateOnSubmit { get; set; }
    [Parameter] public EventCallback HandleOnCancel {  get; set; }
    [Parameter] public string Type {  get; set; }
    [Parameter] public RecordsLibraryModel EditItem { get; set; }

    #endregion

    protected RecordsLibraryModel NewConfig { get; set; } = new RecordsLibraryModel();

    protected FluentValidationValidator? FluentValidationValidator { get; set; }

    protected List<string> SectionList { get; set; } = new List<string>();
    protected List<string> OfficeList { get; set; } = new List<string>();
    protected bool IsSectionEmpty { get; set; } = false;
    protected bool IsOfficeEmpty { get; set; } = false;


    protected TelerikDialog DialogReference = new();    
    #region Submit Events
    protected async Task HandleOnSubmitCallback()
    {
        if(await FluentValidationValidator!.ValidateAsync())
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

    protected async Task LoadCurrentValues()
    {
        if(EditItem.Name != null)
        {
            NewConfig.Name = EditItem.Name;
            NewConfig.Id = EditItem.Id;
            NewConfig.Category = EditItem.Category;
            NewConfig.Section = EditItem.Section;
            NewConfig.Office = EditItem.Office;
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

}
