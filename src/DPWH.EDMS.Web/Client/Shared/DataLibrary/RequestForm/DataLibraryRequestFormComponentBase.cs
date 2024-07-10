using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.Common.Enum;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Shared.DataLibrary.RequestForm;

public class DataLibraryRequestFormComponentBase : RxBaseComponent
{
    [Parameter] public EventCallback<ConfigModel> HandleCreateOnSubmit { get; set; }
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    [Parameter] public string DataType { get; set; }
    [Parameter] public string Type { get; set; }

    [Parameter] public ConfigModel EditItem { get; set; } = default!;
    protected ConfigModel NewConfig { get; set; } = new ConfigModel();
    protected TelerikDialog dialogReference = new();
    //validator
    protected FluentValidationValidator? FluentValidationValidator;
    protected List<string> SectionList { get; set; } = new List<string>();
    protected List<string> OfficeList { get; set; } = new List<string>();
    protected bool IsSectionEmpty { get; set; } = false;
    protected bool IsOfficeEmpty { get; set; } = false;
    protected bool IsVisible {  get; set; } = false;

    #region Load Events

    protected async Task LoadSection()
    {
        SectionList = new List<string>
        {
            "Employee Welfare and Benefits Section",
            "Current Section",
            "Non-Current Section"
        };
    }

    protected async Task LoadOffice()
    {
        OfficeList = new List<string>
        {
            "HRMD",
            "RMD"
        };
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

    #region Optional Validation
    protected bool IsEmployee()
    {
        if (DataType == DataLibraryEnum.EmployeeRecords.ToString())
        {

            if (string.IsNullOrEmpty(NewConfig.Section) || string.IsNullOrEmpty(NewConfig.Office))
            {
                IsSectionEmpty = string.IsNullOrEmpty(NewConfig.Section);
                IsOfficeEmpty = string.IsNullOrEmpty(NewConfig.Office);
                return false;
            }
            else
            {
                return true;
            }
        }
        if (string.IsNullOrEmpty(NewConfig.Value))
        {
            if (string.IsNullOrEmpty(NewConfig.Section) || string.IsNullOrEmpty(NewConfig.Office))
            {
                IsSectionEmpty = string.IsNullOrEmpty(NewConfig.Section);
                IsOfficeEmpty = string.IsNullOrEmpty(NewConfig.Office);
            }
            return false;
        }
        else
        {
            return true;
        }
       
    }
    #endregion
}

