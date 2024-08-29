using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.Common.Enum;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.DataLibrary.Signatories.Components.SignatoryRequestForm;

public class SignatoryRequestFormBase : RxBaseComponent
{
    [Parameter] public EventCallback<SignatoryManagementModel> HandleCreateOnSubmit { get; set; }
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    [Parameter] public string DataType { get; set; }
    [Parameter] public string Type { get; set; }

    [Parameter] public SignatoryManagementModel EditItem { get; set; } = default!;
    protected SignatoryManagementModel NewConfig { get; set; } = new SignatoryManagementModel();
    protected TelerikDialog dialogReference = new();
    //validator
    protected FluentValidationValidator? FluentValidationValidator;
    protected bool IsVisible { get; set; } = false;

    protected override async void OnInitialized()
    {
        IsLoading = true;
        await GetCurrentValues();
        IsVisible = true;
        IsLoading = false;
    }


    protected async Task GetCurrentValues()
    {
        if (Type == "Edit")
        {
            NewConfig.Name = EditItem.Name;
            NewConfig.DocumentType = EditItem.DocumentType;
            NewConfig.Position = EditItem.Position;
            NewConfig.Office1 = EditItem.Office1;
            NewConfig.Office2 = EditItem.Office2;
            NewConfig.SignatoryNo = EditItem.SignatoryNo;
            NewConfig.Id = EditItem.Id;
            NewConfig.EmployeeNumber = EditItem.EmployeeNumber;
        }
        NewConfig.DataType = DataType;
    }
    //protected async Task SectionDropdownErrorChecker()
    //{
    //    IsSectionEmpty = string.IsNullOrEmpty(NewConfig.Section);
    //}
    //protected async Task OfficeDropdownErrorChecker()
    //{
    //    IsOfficeEmpty = string.IsNullOrEmpty(NewConfig.Office);
    //}
    //#endregion

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

}
