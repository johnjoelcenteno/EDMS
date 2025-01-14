﻿using DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
using DPWH.EDMS.Api.Contracts;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Telerik.FontIcons;
using Telerik.Blazor.Components;
using DPWH.EDMS.Client.Shared.APIClient.Services.DpwhIntegrations;
using DPWH.EDMS.Web.Client.Shared.Services.Export;
using Telerik.Blazor.Components.Menu;
using DPWH.EDMS.IDP.Core.Constants;
using Microsoft.AspNetCore.Components.Web;
using DPWH.EDMS.Components.Helpers;
using System.Runtime.InteropServices;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DPWH.EDMS.Web.Client.Pages.UserManagement;

public class UserManagementBase : GridBase<UserModel>
{
    [Inject] protected NavigationManager Nav { get; set; } = default!;
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required ILicensesService LicensesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IDpwhIntegrationsService DpwhIntegrationService { get; set; }
    [Inject] public required IExcelExportService ExcelExportService { get; set; }
    [Parameter] public UserModel ViewItem { get; set; } = default!;


    protected double LicenseLimit = 0;
    protected double LicenseUsed = 0;
    protected double TotalUsers = 0;
    protected string GetOpenBtn = "";
    protected bool IsVisibleDeact = false;
    protected string SelectedAcord { get; set; }
    protected string RespSizer { get; set; }
    protected UserModel SelectedItem { get; set; } = default!;
    protected UserModel UserList { get; set; } = new UserModel();
    protected UserModel userName { get; set; } = new();
    protected ICollection<GetRequestingOfficeResult> RegionOfficeList { get; set; } = new List<GetRequestingOfficeResult>();
    protected List<GetRequestingOfficeResultItem> DEOlist { get; set; } = new List<GetRequestingOfficeResultItem>();
    protected List<GridMenuItemModel> MenuItems { get; set; } = new();
    protected TelerikContextMenu<GridMenuItemModel> ContextMenuRef { get; set; } = new();
    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "group",
            Text = "User Management",
            Url = "/user-management"
        });
    }
    protected void GetGridMenuItems()
    {
        // context menu items
        MenuItems = new List<GridMenuItemModel>
        {
        new (){ Text = "View", Icon=null!, CommandName="View" },
        new (){ Text = "Edit", Icon=null!, CommandName="Edit" },
        new (){ Text = "Deactivate", Icon=null!, CommandName="Deactivate" }
        };
    }
    protected async Task ShowRowOptions(MouseEventArgs e, UserModel row)
    {
        SelectedItem = row;

        await ContextMenuRef.ShowAsync(e.ClientX, e.ClientY);
    }
    protected override async Task OnInitializedAsync()
    {
        //ServiceCb = UserService.Query;
        //await LoadData();
        await LoadUserData();
        GetGridMenuItems();
        await ExceptionHandlerService.HandleApiException(
            async () =>
            {
                var licenseRes = await LicensesService.GetLicenseStatus();

                if (licenseRes.Success)
                {
                    var licenseData = licenseRes.Data;
                    LicenseUsed = licenseData.Limit - licenseData.Available;
                    //LicenseUsed = 1; //Temporary Value for used license
                    LicenseLimit = licenseData.Limit;
                    TotalUsers = licenseData.EndUsersCount;
                }
            });
    }

    protected async Task LoadUserData()
    {
        IsLoading = true;
        var result = await UserService.Query(DataSourceReq);
        if (result.Data != null)
        {
            var getData = GenericHelper.GetListByDataSource<UserModel>(result.Data);
            GridData = getData;
            IsLoading = false;
        }
       
    }
    protected double GetLicenseAccumulatedPercentage()
    {
        return Math.Round((LicenseUsed / LicenseLimit) * 100, 2);
    }
    protected async Task AddUser()
    {
        NavManager.NavigateTo("user-management/add");
    }
    protected void OnItemClick(GridMenuItemModel item)
    {
        // one way to pass handlers is to use an Action, you don't have to use this
        if (item.Action != null)
        {
            item.Action.Invoke();
        }
        else
        {
            // or you can use local code to perform a task
            // such as put a row in edit mode or select it
            SelectedAcord = item.CommandName;
            var guid = Guid.Parse(SelectedItem.Id);

            switch (item.CommandName)
            {

                case "View":
                    var viewUrl = $"{Nav.BaseUri}user-management/view-layout/{SelectedItem.Id}";
                    Nav.NavigateTo(viewUrl);
                    break;
                case "Edit":
                    var EditUrl = $"{Nav.BaseUri}user-management/edit/{SelectedItem.Id}";
                    Nav.NavigateTo(EditUrl);
                    break;
                case "Deactivate":
                    IsVisibleDeact = true; 
                    userName = SelectedItem; 
                    break;
                default:
                    break;
            }
        }

    }
    protected async Task OnSelectUser(UserModel user)
    {
        var guid = Guid.Parse(user.Id);
        var req = new DeactivateUserCommand
        {
            UserId = guid,
            Reason = "Reason placeholder"
        };
        //try
        //{

        var result = await ExceptionHandlerService.HandleApiException<DeactivateUserResultBaseApiResponse>(async () => await UserService.DeactivateUser(req), null);
        if (result != null && result.Success)
        {
            await OnInitializedAsync();
            ToastService.ShowSuccess($"{user.UserName} Account Deactivated");
            IsVisibleDeact = false;
        }
        //}
        //catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
        //{
        //    var problemDetails = apiExtension.Result;
        //    var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
        //    ToastService.ShowError(error);
        //}

    }

    protected async Task ConfirmToExcel()
    {
        
        IsLoading = true;
        try
        {
            var fileName = $"User Management Reports as of {DateTime.Now.ToString("MMM dd, yyyy")}.xlsx";
            await ExceptionHandlerService.HandleApiException(async () => await ExcelExportService.ExportList(GridData, fileName), null);
            //await ExcelExportService.ExportList(GridData, fileName);
 
        }
        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
        {
            var problemDetails = apiExtension.Result;
            var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
            ToastService.ShowError(error);
        }

        IsLoading = false;
    }
}
