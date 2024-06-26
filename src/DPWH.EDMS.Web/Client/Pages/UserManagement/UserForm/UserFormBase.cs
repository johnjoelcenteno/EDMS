using DPWH.EDMS.Components;
using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.BlazoredFluentValidator;
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Telerik.Blazor.Components;
using System.Linq;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using Microsoft.AspNetCore.Components.Authorization;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using AutoMapper;
using DPWH.EDMS.Client.Shared.APIClient.Services.DpwhIntegrations;
using DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;

namespace DPWH.EDMS.Web.Client.Pages.UserManagement.UserForm;

public class UserFormBase : RxBaseComponent
{
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required ILicensesService LicensesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IDpwhIntegrationsService DpwhIntegrationService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }

    protected UserManagementModel User { get; set; } = new();
    protected UserManagementModel UserModel { get; set; }
    protected TelerikForm FormRef { get; set; } = new();
    protected string SelectedAcord { get; set; }
    protected bool OnEmpId { get; set; } = true;
    protected bool OnSearchPis { get; set; } = true;
    protected FluentValidationValidator? FluentValidationValidator;
    protected ICollection<GetRequestingOfficeResult> RegionOfficeList { get; set; } = new List<GetRequestingOfficeResult>();
    protected List<GetRequestingOfficeResultItem> Deolist { get; set; } = new List<GetRequestingOfficeResultItem>();

    protected void ClearSearch()
    {
        User.EmployeeId = null;
    }
    protected async Task OnSearchEmployeeID(string id)
    {
        IsLoading = true;
        if (id != null)
        {
            await ExceptionHandlerService.HandleApiException(
            async () =>
            {
                var res = await DpwhIntegrationService.GetByEmployeeId(id);
                if (res.Success)
                {
                    UserModel = new UserManagementModel();
                    UserModel.EmployeeId = id;
                    UserModel.FirstName = res.Data.FirstName;
                    UserModel.MiddleName = res.Data.MiddleInitial;
                    UserModel.LastName = res.Data.FamilyName;
                    UserModel.Email = res.Data.NetworkId + "@dpwh.gov.ph";
                    UserModel.Position = res.Data.PlantillaPosition;
                    UserModel.DesignationTitle = res.Data.DesignationTitle;

                }
                else
                {
                    ToastService.ShowError(id + " not found");
                }

                var fullName = UserModel.LastName + ", " + UserModel.FirstName + " " + UserModel.MiddleName;
                User.FullName = fullName;

                SelectedAcord = "add";
                User.FirstName = UserModel.FirstName;
                User.MiddleName = UserModel.MiddleName;
                User.LastName = UserModel.LastName;
                User.Email = UserModel.Email;
                User.Position = UserModel.Position;
                User.DesignationTitle = UserModel.DesignationTitle;
                ClearNotif();
            });
            //catch (ApiException ex)
            //{
            //    if (ex.StatusCode == 404)
            //    {
            //        //ToastService.ShowError(id + " not found");
            //        OnEmpId = false;
            //        OnSearchPis = true;
            //        ClearSearch();
            //    }
            //    else if (ex.StatusCode == 500)
            //    {
            //        OnSearchPis = false;
            //        OnEmpId = true;
            //        await OnSearchEmployeeID(id);
            //    }
            //    else
            //    {
            //        await OnSearchEmployeeID(id);
            //    }
            //}

        }
        else
        {
            SelectedAcord = "add";
            User.FirstName = string.Empty;
            User.LastName = string.Empty;
            User.FullName = string.Empty;
            User.Position = string.Empty;
            User.DesignationTitle = string.Empty;
            User.Role = string.Empty;
            User.Email = string.Empty;

        }
        IsLoading = false;
    }
    protected void ClearNotif()
    {
        OnEmpId = true;
        OnSearchPis = true;
    }
    private void _ResetDropDownOnOfficeChanged()
    {
        Deolist = new List<GetRequestingOfficeResultItem>();
    }
}

