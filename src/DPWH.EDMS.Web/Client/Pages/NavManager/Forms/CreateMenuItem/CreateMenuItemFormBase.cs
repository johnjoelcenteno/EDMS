using AutoMapper;
using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;
using DPWH.EDMS.Components;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.NavManager.Forms.CreateMenuItem;

public class CreateMenuItemFormBase : RxBaseComponent
{
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required INavigationService NavigationService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }

    [Parameter] public EventCallback HandleOnCancel { get; set; }
    protected CreateMenuItemModel SelectedItem { get; set; } = new();
    protected MenuItemModel? SelectedParent { get; set; }

    // Lists
    protected List<MenuItemModel> MenuItemList = new();

    protected List<string> NavTypeList = new List<string>() {
        NavType.MainMenu.ToString(),
        NavType.CurrentUserMenu.ToString(),
        NavType.Settings.ToString()
    };

    protected List<string> AuthorizedRoleList = new List<string>() {
        ApplicationRoles.SuperAdmin,
        ApplicationRoles.SystemAdmin,
        ApplicationRoles.Manager,
        ApplicationRoles.ITSupport,
        ApplicationRoles.Staff,
        ApplicationRoles.EndUser,
        ApplicationRoles.Deactivated,
    };

    protected List<string> SelectedAuthorizedRoleList = new List<string>();

    // Dropdowns
    //protected TelerikDropDownList<GetValidIDsResult, string> ParentIdsRef = new();

    // Validator
    protected FluentValidationValidator? FluentValidationValidator;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;

        // set defaults
        SelectedItem.AuthorizedRoles = new List<string>();
        SelectedItem.Expanded = false;

        //SelectedItem.AuthorizedRoles = new List<string>();
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

    protected async Task HandleParentSelect()
    {
        IsLoading = true;
        Guid parentId = SelectedItem.ParentId ?? Guid.Empty;
        if (GenericHelper.IsGuidHasValue(parentId))
        {
            var parentRes = await NavigationService.GetById(parentId);
            if (parentRes.Success)
            {
                SelectedParent = parentRes.Data;
                SelectedItem.AuthorizedRoles = SelectedParent.AuthorizedRoles;
                SelectedAuthorizedRoleList = SelectedItem.AuthorizedRoles.ToList();
                SelectedItem.Level = SelectedParent.Level + 1;
                SelectedItem.NavType = SelectedParent.NavType;
                StateHasChanged();
            }
        }
        else
        {
            SelectedParent = null;  
        }
        IsLoading = false;
    }
    protected void HandleSelectRoles()
    {
        SelectedItem.AuthorizedRoles = SelectedAuthorizedRoleList;
    }
    protected void HandleOnCancelCallback()
    {
        NavManager.NavigateTo("/navmanager");
    }
    #endregion
}
