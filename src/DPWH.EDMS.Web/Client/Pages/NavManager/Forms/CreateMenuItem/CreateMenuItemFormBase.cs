using AutoMapper;
using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.NavManager.Forms.CreateMenuItem;

public class CreateMenuItemFormBase : RxBaseComponent
{
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required INavigationService NavigationService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }

    [Parameter] public bool IsEditMode { get; set; } = false;
    //[Parameter] public EventCallback<(CreateRecordRequest, UploadRecordRequestDocumentModel, UploadRecordRequestDocumentModel)> HandleCreateOnSubmit { get; set; }
    //[Parameter] public EventCallback<CreateRecordRequest> HandleEditOnSubmit { get; set; } // TODO
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    protected CreateMenuItemModel SelectedItem { get; set; } = new();

    // Lists
    protected List<MenuItemModel> MenuItemList = new();
    protected List<string> NavTypeList = new List<string>() { 
        NavType.MainMenu.ToString(), 
        NavType.CurrentUserMenu.ToString(), 
        NavType.Settings.ToString() 
    };

    // Dropdowns
    //protected TelerikDropDownList<GetValidIDsResult, string> ParentIdsRef = new();

    // Validator
    protected FluentValidationValidator? FluentValidationValidator;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;

        SelectedItem.Expanded = false;
        SelectedItem.AuthorizedRoles = new List<string>();
        //SelectedItem.NavType = NavType.MainMenu.ToString();

        await LoadMenuItems();

        IsLoading = false;
    }

    #region Load Events
    protected async Task LoadMenuItems()
    {
        var res = await NavigationService.Query(new DataSourceRequest() { Skip = 0 });


        if (res.Errors == null)
        {
            var list = GenericHelper.GetListByDataSource<MenuItemModel>(res.Data);
            MenuItemList = list;
        }
        else
        {
            ToastService.ShowError("Something went wrong on loading menus");
        }
    }
    #endregion

    #region Submit Events
    protected async Task HandleOnSubmitCallback()
    {
        if (await FluentValidationValidator!.ValidateAsync())
        {
            var res = await NavigationService.Create(SelectedItem);
            if (res.Success)
            {
                ToastService.ShowSuccess("Successfully created menu item!");
                NavManager.NavigateTo("/navmanager");
            }
            else
            {
                ToastService.ShowError("Something went wront on creating menu item!");
            }
        }
    }
    protected void HandleOnCancelCallback()
    {
        NavManager.NavigateTo("/navmanager");
    }
    #endregion
}
