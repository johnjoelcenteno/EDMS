using AutoMapper;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.Common.Enum;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using Filter = DPWH.EDMS.Api.Contracts.Filter;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.IDP.Core.Constants;
using Telerik.SvgIcons;

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
    public TelerikAutoComplete<UserModel> EmployeeAutoCompleteRef { get; set; } = new();
    protected List<UserModel> UserDataList = new List<UserModel>();
    public CancellationTokenSource _tokenSource = new(); // for debouncing the service calls
    public UserModel? SelectedEmployee { get; set; } = new(); 
    [Inject] public IMapper _mapper { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    public string SelectedEmployeeId { get; set; } = string.Empty;
    protected bool OnSearch { get; set; } = true;
    protected bool IsVisible { get; set; } = false;
    protected bool isEdit { get; set; } = true;

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
            isEdit = false;
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

    public async Task OnEmployeeAutoCompleteValueChanged(string newValue)
    {
        SelectedEmployeeId = newValue;

        if (!string.IsNullOrWhiteSpace(newValue))
        {
            await _DebounceDelay();
            // for fetching data
            var filter = new Filter
            {
                Logic = "or",
                Filters = new List<Filter>
                {
                       new Filter
                        {
                            Field = nameof(UserModel.EmployeeId),
                            Operator = "contains",
                            Value = newValue,
                        },
                        new Filter
                        {
                            Field = nameof(UserModel.FirstName),
                            Operator = "contains",
                            Value = newValue,
                        }
                        ,
                        new Filter
                        {
                            Field = nameof(UserModel.LastName),
                            Operator = "contains",
                            Value = newValue,
                        }
                }
            };

            //await UsersService.Query(new DataSourceRequest() { Filter = filter });
            var res = await UsersService.Query(new DataSourceRequest() { Filter = filter });
            if (res != null && res.Data.Count != 0)
            {

                var result = GenericHelper.GetListByDataSource<UserModel>(res.Data);
                UserDataList = _mapper.Map<List<UserModel>>(result.Where(result => result.UserAccess == "Super Admin" || result.UserAccess == "Manager"));

                EmployeeAutoCompleteRef.Rebind();
                OnSearch = true;
                
            }
            else
            {
                
                UserDataList.Clear();
                EmployeeAutoCompleteRef.Rebind();
            }

        }
    } 
    private async Task _DebounceDelay()
    {
        // debouncing
        _tokenSource.Cancel();
        _tokenSource.Dispose();

        _tokenSource = new CancellationTokenSource();
        var token = _tokenSource.Token;

        await Task.Delay(100, token); // 300ms timeout for the debouncing
    }


    protected void OnEmployeeChanged()
    {
        //if (SelectedEmployee is null) return;

        SelectedEmployee = UserDataList.FirstOrDefault(c => c.Employee?.ToUpper() == SelectedEmployeeId.ToUpper());

        if (SelectedEmployee == null)
        {
            if (NewConfig.EmployeeNumber == null)
            {
                OnSearch = false;
                UserDataList.Clear();
            }
        }
        else
        {
            NewConfig.Name = SelectedEmployee.EmployeeFullName;
            NewConfig.EmployeeNumber = SelectedEmployee.EmployeeId;
            OnSearch = true;
            SelectedEmployeeId = string.Empty;
        }

        if (NewConfig.Name == SelectedEmployeeId)
        {
            OnSearch = true;
            NewConfig.EmployeeNumber = SelectedEmployee.EmployeeId;
            NewConfig.Name = SelectedEmployee.EmployeeFullName;
            SelectedEmployeeId = string.Empty;
        }
        dialogReference.Refresh();
        FluentValidationValidator!.ValidateAsync();
    }
    protected List<string> ListOfOffice1 = new List<string>()
        {
            "Current Section",
            "Non-Current Section",
            "Office of the Chief",
            "N/A"
        };

    protected List<string> ListOfDocumentType = new List<string>()
        {
            "Current Document",
            "Non-Current Document",
            "Certificate of No Records Found",
            "All" 
        };
}
