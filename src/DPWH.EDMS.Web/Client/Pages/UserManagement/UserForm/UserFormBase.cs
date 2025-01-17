﻿using DPWH.EDMS.Components;
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
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
using AutoMapper;
using DPWH.EDMS.Client.Shared.APIClient.Services.DpwhIntegrations;
using DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandlerPIS;

using DPWH.EDMS.IDP.Core.Constants;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Office2010.Excel;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandlerEmployee;

namespace DPWH.EDMS.Web.Client.Pages.UserManagement.UserForm;

public class UserFormBase : RxBaseComponent
{
    [Inject] public required ILookupsService LookupService { get; set; }
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required ILicensesService LicensesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IExceptionPISHandlerService ExceptionPISHandlerService { get; set; }
    [Inject] public required IExceptionHandlerEmployeeService ExceptionHandlerEmployeeService { get; set; }
    [Inject] public required IDpwhIntegrationsService DpwhIntegrationService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    [Parameter] public EventCallback HandleCreateOnSubmit { get; set; }
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    [Parameter] public string Type { get; set; }
    protected UserManagementModel User { get; set; } = new();
    protected UserManagementModel UserModel { get; set; }
    protected TelerikForm FormRef { get; set; } = new();
    protected TelerikDialog dialogReference = new();
    public string SelectedRegionalOffice { get; set; }
    public string SelectedImplementingOffice { get; set; }
    public string SelectedUserRole { get; set; }
    protected string UserRole { get; set; }
    protected string Role { get; set; }
    protected string RegionOffice { get; set; }
    protected string DistrictOffice { get; set; }
    protected string CentralOffice = "Central Office";
    protected string OnSearchEmployeeId { get; set; }
    //protected string SelectedOffice { get; set; }
    protected string LicenseInfo { get; set; }
    protected string UserName { get; set; }
    protected bool OnEmpRole { get; set; } = true;
    protected bool OnEmpId { get; set; } = true;
    protected bool OnSearchPis { get; set; } = true;
    protected bool OnRegion { get; set; } = true;
    protected bool OnDistrict { get; set; } = true;
    protected bool IsSaving { get; set; } = false;
    protected bool IsManager { get; set; } = false;
    protected bool UserCategory { get; set; } = false;
    protected bool OnSearched { get; set; } = false;
    protected bool ExistingUser { get; set; } = false;

    protected double LicenseLimit = 0;
    protected double LicenseUsed = 0;
    protected double TotalUsers = 0;
    protected FluentValidationValidator? FluentValidationValidator;
    protected ICollection<GetRequestingOfficeResult> RegionOfficeList { get; set; } = new List<GetRequestingOfficeResult>();
    protected List<GetRequestingOfficeResultItem> Deolist { get; set; } = new List<GetRequestingOfficeResultItem>();
    protected TelerikDropDownList<GetRequestingOfficeResult, string> RegionOfficeRef = new();
    protected TelerikDropDownList<GetRequestingOfficeResultItem, string> ImplementRef = new();
    protected TelerikDropDownList<UserAccessRoles, string> DropDownListRef { get; set; } = new();
    protected List<UserAccessRoles> UserAccessList { get; set; } = new List<UserAccessRoles>();
    [Parameter] public string Id { get; set; }
    protected Guid UserId = new Guid();

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        if (Type == "View" || Type == "Edit")
        {
            if (Guid.TryParse($"{Id}", out var id))
            {
                UserId = id;
            }

            var selectedUser = await UserService.GetById(id);
            if (selectedUser.Success)
            {
                User.FullName = $"{selectedUser.Data.LastName}, {selectedUser.Data.FirstName} {selectedUser.Data.MiddleInitial}";
                User.FirstName = selectedUser.Data.FirstName;
                User.MiddleName = selectedUser.Data.MiddleInitial;
                User.LastName = selectedUser.Data.LastName;
                User.EmployeeId = selectedUser.Data.EmployeeId;
                User.Email = selectedUser.Data.Email;
                User.Position = selectedUser.Data.Position;
                User.DesignationTitle = selectedUser.Data.DesignationTitle;
                User.RegionalOffice = selectedUser.Data.RegionalOfficeRegion;
                User.DistrictEngineeringOffice = selectedUser.Data.DistrictEngineeringOffice;
                User.Role = selectedUser.Data.Role;
                User.UserAccess = selectedUser.Data.UserAccess;
                User.Office = selectedUser.Data.Office;
                User.Created = selectedUser.Data.CreatedDate;
                User.CreatedBy = selectedUser.Data.CreatedBy;
            }
            OnSelectedChange(true);
        }
        await ExceptionHandlerService.HandleApiException(
            async () =>
            {
                var licenseRes = await LicensesService.GetLicenseStatus();

                if (licenseRes.Success)
                {
                    var licenseData = licenseRes.Data;
                    //LicenseUsed = licenseData.Limit - licenseData.Available;
                    LicenseUsed = 1; //Temporary Value for used license
                    LicenseLimit = licenseData.Limit;
                    TotalUsers = licenseData.EndUsersCount;
                }

                UserAccessList = ApplicationRoles.AssignableRoles.Select(ar => new UserAccessRoles
                {
                    idRole = ar.Key,
                    UserAccess = ar.Value
                }).ToList();
            });

        await GetRegionList();
        if (Type == "View")
        {
            DropDownListRef.DefaultText = User.UserAccess;
            User.Role = User.UserAccess;
        }
        else if (Type == "Edit")
        {
            SelectedUserRole = User.Role;
            dialogReference.Refresh();
            DropDownListRef.DefaultText = SelectedUserRole;
            OnSearched = true;
        }
        SelectedRegionalOffice = User.RegionalOffice;
        SelectedImplementingOffice = User.DistrictEngineeringOffice;
        await OnRegionOfficeChanged();
        IsLoading = false;
        dialogReference.Refresh();
    }
    protected List<string> Offices = new List<string>()
        {
            "RMD",
            "HRMD"
        };
    protected void OnCategoryChange()
    {
        //Pending for Changes
    }
    protected class UserAccessRoles
    {
        public string idRole { get; set; }
        public string UserAccess { get; set; }
    }
    protected void ClearSearch()
    {
        User.EmployeeId = null;
        SelectedRegionalOffice = null;
        SelectedImplementingOffice = null;

        User.Position = string.Empty;
        User.FullName = string.Empty;
        User.DesignationTitle = string.Empty;
        User.RegionalOffice = "Select";
        User.DistrictEngineeringOffice = "Select";

        User.FirstName = string.Empty;
        User.LastName = string.Empty;
        User.Role = "Select";
        User.Office = string.Empty;

        User.Email = string.Empty;
        UserCategory = false;
        OnSearched = false;
    }
    protected async Task OnSearchEmployeeID(string id)
    {
        IsLoading = true;
        if (id != null)
        {
            await ExceptionHandlerEmployeeService.HandleApiException(
           async () =>
           {
               var res = await UsersService.GetUserByEmployeeId(id);
               if (res.Success)
               {
                   if (res.Data != null)
                   {
                       UserModel = new UserManagementModel();
                       UserModel.EmployeeId = id;
                       UserModel.FirstName = res.Data.FirstName;
                       UserModel.MiddleName = res.Data.MiddleInitial;
                       UserModel.LastName = res.Data.LastName;
                       UserModel.Email = res.Data.UserName;
                       UserModel.Position = res.Data.Position;
                       UserModel.DesignationTitle = res.Data.DesignationTitle;
                       UserModel.RegionalOffice = res.Data.RegionalOfficeRegion;
                       UserModel.DistrictEngineeringOffice = res.Data.DistrictEngineeringOffice;

                       OnSearched = true;
                       OnEmpId = true;
                       //user exist on NGOBIA
                       ExistingUser = true;
                   }
                   var fullName = UserModel.LastName + ", " + UserModel.FirstName + " " + UserModel.MiddleName;
                   User.FullName = fullName;

                   User.FirstName = UserModel.FirstName;
                   if (UserModel.MiddleName == null)
                   {
                       User.MiddleName = string.Empty;
                   }
                   else
                   {
                       User.MiddleName = UserModel.MiddleName;
                   } 
                   User.LastName = UserModel.LastName;
                   User.Email = UserModel.Email;
                   User.Position = UserModel.Position;
                   User.DesignationTitle = UserModel.DesignationTitle;
                   User.RegionalOffice = UserModel.RegionalOffice;
                   User.DistrictEngineeringOffice = UserModel.DistrictEngineeringOffice;
                   SelectedRegionalOffice = User.RegionalOffice;
                   SelectedImplementingOffice = User.DistrictEngineeringOffice;
                   await OnRegionOfficeChanged();

               }

           }, null, null, true, async (bool empId, bool pisSearch) => await EmployeeException(empId, pisSearch)
                );
        }
        else
        {
            User.FirstName = string.Empty;
            User.LastName = string.Empty;
            User.FullName = string.Empty;
            User.Position = string.Empty;
            User.DesignationTitle = string.Empty;
            User.Role = string.Empty;
            User.Email = string.Empty;
            SelectedRegionalOffice = string.Empty;
            SelectedImplementingOffice = string.Empty;
        }

        IsLoading = false;
        //await LoadUserRegion();
        OnSearchEmployeeId = id;
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

    protected async Task PISException(bool empId, bool searchPIS)
    {
        // status code 400
        if (OnEmpId)
        {
            OnEmpId = empId;
            OnSearchPis = searchPIS;
            ClearSearch();

        }
        // status code 404 or any status code
        else
        {
            OnEmpId = empId;
            OnSearchPis = searchPIS;
            await OnSearchEmployeeID(User.EmployeeId);
        }
    }
    protected async Task EmployeeException(bool empId, bool searchPIS)
    {
        // status code 500
        if (OnEmpId)
        { 
            await ExceptionPISHandlerService.HandleApiException(
           async () =>
           {
               var res = await DpwhIntegrationService.GetByEmployeeId(User.EmployeeId);
               if (res.Success)
               {
                   UserModel = new UserManagementModel();
                   UserModel.EmployeeId = User.EmployeeId;
                   UserModel.FirstName = res.Data.FirstName;
                   UserModel.MiddleName = res.Data.MiddleInitial;
                   UserModel.LastName = res.Data.FamilyName;
                   UserModel.Email = res.Data.NetworkId + "@dpwh.gov.ph";
                   UserModel.Position = res.Data.PlantillaPosition;
                   UserModel.DesignationTitle = res.Data.DesignationTitle;
                   OnSearched = true;
                   ExistingUser = false;
               }
               else
               {
                   ToastService.ShowError(User.EmployeeId + " not found");
               }

               var fullName = UserModel.LastName + ", " + UserModel.FirstName + " " + UserModel.MiddleName;
               User.FullName = fullName;
 
               User.FirstName = UserModel.FirstName;
               if (UserModel.MiddleName == null)
               {
                   User.MiddleName = string.Empty;
               }
               else
               {
                   User.MiddleName = UserModel.MiddleName;
               }
               User.LastName = UserModel.LastName;
               User.Email = UserModel.Email;
               User.Position = UserModel.Position;
               User.DesignationTitle = UserModel.DesignationTitle;
               ClearNotif();
               OnEmpId = true;
           }, null, null, true, async (bool empId, bool pisSearch) => await PISException(empId, pisSearch)
               );
        }
        // status code 404 or any status code
        else
        {
            OnEmpId = empId;
            OnSearchPis = searchPIS;
            await OnSearchEmployeeID(User.EmployeeId);
        }
    }
    protected async Task OnRegionOfficeChanged()
    {

        if (RegionOfficeList != null && SelectedRegionalOffice != null)
        {
            try
            {
                var selectedRegion = RegionOfficeList.FirstOrDefault(item => item.RegionName == SelectedRegionalOffice);

                if (selectedRegion != null)
                {
                    SelectedRegionalOffice = selectedRegion.RegionName;
                    User.RegionalOffice = SelectedRegionalOffice;
                    _ResetDropDownOnOfficeChanged();
                    Deolist = selectedRegion.ImplementingOffices.ToList();
                    OnRegion = true;
                }
                else
                {
                    OnRegion = false;
                }
            }
            catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
            {
                var problemDetails = apiExtension.Result;
                var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
                ToastService.ShowError(error);
            }
        }
        else
        {
            if (Type != "Add")
            {
                OnRegion = false;
            }
            _ResetDropDownOnOfficeChanged();
        }
    }
    protected async Task OnChangeOffice()
    {
        try
        {
            if (Deolist != null && SelectedImplementingOffice != null)
            {
                SelectedImplementingOffice = Deolist.FirstOrDefault(p => p.SubOfficeName == SelectedImplementingOffice)!.SubOfficeName;
                User.DistrictEngineeringOffice = SelectedImplementingOffice;
                OnDistrict = true;
            }
            else
            { OnDistrict = false; }

            ImplementRef.Refresh();
        }
        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
        {
            var problemDetails = apiExtension.Result;
            var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
            ToastService.ShowError(error);
        }

        StateHasChanged();
    }
    protected async Task GetRegionList()
    {

        try
        {
            var regionOfficeList = await LookupService.GetRequestingOfficeList();

            if (regionOfficeList != null)
            {
                RegionOfficeList = regionOfficeList.Data
                    .Select(item => new GetRequestingOfficeResult
                    {
                        RegionId = item.RegionId,
                        RegionName = item.RegionName,
                        ImplementingOffices = item.ImplementingOffices,

                    }).ToList();
            }
            else
            {
                RegionOfficeList = new List<GetRequestingOfficeResult>();
            }
        }
        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
        {
            var problemDetails = apiExtension.Result;
            var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
            ToastService.ShowError(error);
        }
    }
    protected void OnSelectedChange(bool selected)
    {
        if (selected)
        {
            LicenseInfo = ApplicationPolicies.NoLicenseUsers.Contains(User.Role) ?
                "This will not consume a license" :
                "This role consumes a license";
            OnEmpRole = true;
        }
        if (Role == ApplicationRoles.Manager)
        {
            var selectedRegion = RegionOfficeList.FirstOrDefault(item => item.RegionName == SelectedRegionalOffice);
            if (User.Role == "dpwh_manager")
            {
                if (selectedRegion != null)
                {
                    SelectedRegionalOffice = selectedRegion.RegionName;
                    User.RegionalOffice = SelectedRegionalOffice;
                    _ResetDropDownOnOfficeChanged();
                    Deolist = selectedRegion.ImplementingOffices.Where(a => a.SubOfficeName != SelectedRegionalOffice).ToList();
                    OnRegion = true;
                }
            }
            else
            {
                if (selectedRegion != null)
                {
                    SelectedRegionalOffice = selectedRegion.RegionName;
                    User.RegionalOffice = SelectedRegionalOffice;
                    _ResetDropDownOnOfficeChanged();
                    Deolist = selectedRegion.ImplementingOffices.ToList();
                    OnRegion = true;
                }
            }
            OnEmpRole = true;
        }
        if (User.Role == ApplicationRoles.Manager || User.Role == ApplicationRoles.Staff)
        {
            UserCategory = true;
        }
        else
        {
            UserCategory = false;
        }
    }
    protected async Task HandleOnCancelCallback()
    {
        if (HandleOnCancel.HasDelegate)
        {
            await HandleOnCancel.InvokeAsync();
        }
    }
    protected void OnCancel()
    {
        NavManager.NavigateTo("/user-management");
    }
    protected async Task AddUser(UserManagementModel user)
    {
        IsLoading = true;
        //validateRegion();
        if (OnRegion != false && OnDistrict != false)
        {
            if (User.Role != null)
            {
                if (User.RegionalOffice == null || User.RegionalOffice == "Select")
                {
                    user.RegionalOffice = string.Empty;
                }
                else
                {
                    user.RegionalOffice = User.RegionalOffice;
                }
                if (User.DistrictEngineeringOffice == null || User.DistrictEngineeringOffice == "Select")
                {
                    user.DistrictEngineeringOffice = string.Empty;
                }
                else
                {
                    user.DistrictEngineeringOffice = User.DistrictEngineeringOffice;
                }

                IsSaving = true;

                if (LicenseLimit <= 0 && User.Role != ApplicationRoles.EndUser)
                {
                    ToastService.ShowWarning("Insufficient License!");
                    IsLoading = false;
                    IsSaving = false;
                    return;
                }

                var newUsers = new CreateUserWithRoleCommand
                {
                    EmployeeId = user.EmployeeId,
                    FirstName = user.FirstName,
                    MiddleInitial = user.MiddleName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = user.Role,
                    RegionalOfficeRegion = user.RegionalOffice,
                    RegionalOfficeProvince = user.RegionalOffice,
                    DistrictEngineeringOffice = user.DistrictEngineeringOffice,
                    DesignationTitle = user.DesignationTitle,
                    MobileNumber = string.Empty,
                    Department = user.Department,
                    Position = user.Position,
                    Office = user.Office
                };
                try
                {
                    if (newUsers != null)
                    {
                        var res = await UserService.CreateUserWithRole(newUsers);
                        ToastService.ShowSuccess("User added successfully!");
                        User.EmployeeId = string.Empty;
                        NavManager.NavigateTo("/user-management");

                    }
                }
                catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtention)
                {
                    var problemDetails = apiExtention.Result;
                    if (problemDetails.AdditionalProperties.ContainsKey("error"))
                    {
                        var error = problemDetails.AdditionalProperties["error"].ToString();
                        ToastService.ShowError(error ?? string.Empty);
                    }

                    else if (problemDetails.AdditionalProperties.ContainsKey("errors"))
                    {
                        var errors = problemDetails.AdditionalProperties["errors"].ToString();
                        List<Dictionary<string, object>> errorList =
                            JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(errors ?? string.Empty)!;
                        if (errorList != null && errorList.Count > 0)
                        {
                            string list = "";

                            foreach (var error in errorList)
                            {
                                var errorMessage = error["errorMessage"].ToString();
                                list += $@"<li>{errorMessage}</li>";
                            }

                            var htmlDisplay = new MarkupString($@"<ul>{list}</ul>").ToString();

                            var htmlContent = new RenderFragment(builder =>
                            {
                                builder.AddMarkupContent(0, htmlDisplay);
                            });

                            ToastService.ShowError(htmlContent);
                        }
                    }
                }
                catch (Exception ex) when (ex is ApiException apiExtention)
                {
                    var htmlContent = new RenderFragment(builder =>
                    {
                        builder.AddMarkupContent(0, apiExtention.Message);
                    });
                    ToastService.ShowError(htmlContent);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError("Email Already Exist " + user.Email);
                }
                FormRef.Refresh();
                OnEmpId = true;

            }
            else
            {
                OnEmpRole = false;
                FormRef.Refresh();
            }
        }
        if (IsManager == false)
        {
            User.RegionalOffice = "Select";
            User.DistrictEngineeringOffice = "Select";
            SelectedRegionalOffice = null;
            SelectedImplementingOffice = null;
            IsLoading = false;
            IsSaving = false;
        }
        else
        {
            IsLoading = false;
            IsSaving = false;
        }
    }
    protected async Task EditUser(UserManagementModel editedUser)
    {
        IsLoading = true;
        validateRegion();
        if (OnRegion != false && OnDistrict != false)
        {
            if (string.IsNullOrEmpty(User.RegionalOffice) || User.RegionalOffice == "Select")
            {
                editedUser.RegionalOffice = string.Empty;
            }

            editedUser.Department = string.Empty;

            if (string.IsNullOrEmpty(User.DistrictEngineeringOffice) || User.DistrictEngineeringOffice == "Select")
            {
                editedUser.DistrictEngineeringOffice = string.Empty;
            }

            IsSaving = true;
            IsLoading = true;

            if (LicenseLimit <= 0)
            {
                ToastService.ShowWarning("Insufficient License!");
                IsSaving = false;
                IsLoading = false;
                return;
            }
            var update = new UpdateUserCommand
            {
                UserId = UserId,
                FirstName = editedUser.FirstName,
                LastName = editedUser.LastName,
                MobileNumber = string.Empty,
                EmployeeId = editedUser.EmployeeId,
                Department = string.Empty,
                Position = editedUser.Position,
                Role = editedUser.Role,
                RegionalOfficeRegion = editedUser.RegionalOffice,
                RegionalOfficeProvince = editedUser.RegionalOffice,
                DistrictEngineeringOffice = editedUser.DistrictEngineeringOffice,
                DesignationTitle = string.Empty,
                Office = editedUser.Office
            };

            if (update != null)
            {
                try
                {
                    var res = await UserService.Update(UserId, update);
                    ToastService.ShowSuccess("User Updated successfully!");
                    User.EmployeeId = string.Empty;
                    NavManager.NavigateTo("/user-management");
                }
                catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
                {
                    var problemDetails = apiExtension.Result;
                    var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
                    ToastService.ShowError(error);
                }

            }
        }
        //await LoadUser();
        IsSaving = false;
        IsLoading = false;
    }
    private async Task LoadUserRegion()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        UserName = user.Identity.Name;

        if (user.Identity!.IsAuthenticated)
        {
            var claims = user.Claims.ToList();

            var roleClaim = claims.FirstOrDefault(x => x.Type == "role");
            Role = roleClaim?.Value ?? string.Empty;

            var regionalOfficeClaim = claims.FirstOrDefault(x => x.Type == "regional_office");
            RegionOffice = regionalOfficeClaim?.Value ?? string.Empty;

            var districtOfficeClaim = claims.FirstOrDefault(x => x.Type == "implementing_office");
            DistrictOffice = districtOfficeClaim?.Value ?? string.Empty;

            var filteredRegionalOfficeProvinces = claims.FirstOrDefault(x => x.Type == "implementing_office");

            if (filteredRegionalOfficeProvinces != null)
            {
                User.DistrictEngineeringOffice = filteredRegionalOfficeProvinces.Value;
            }
            if (RegionOffice != null)
            {
                await GetRegionList();
                SelectedRegionalOffice = RegionOffice;
                await OnRegionOfficeChanged();
            }
        }

        if (Role == ApplicationRoles.Manager)
        {
            UserAccessList = ApplicationRoles.AssignableRoles
                .Where(ar => ar.Key != ApplicationRoles.SuperAdmin && ar.Key != ApplicationRoles.ITSupport)
                .Select(ar => new UserAccessRoles
                {
                    idRole = ar.Key,
                    UserAccess = ar.Value
                }).ToList();
        }
        else
        {
            UserAccessList = ApplicationRoles.AssignableRoles.Select(ar => new UserAccessRoles
            {
                idRole = ar.Key,
                UserAccess = ar.Value
            }).ToList();
        }
    }
    protected void validateRegion()
    {
        if (string.IsNullOrEmpty(User.RegionalOffice) || User.RegionalOffice == "Select")
        {
            OnRegion = false;
        }
        if (string.IsNullOrEmpty(User.DistrictEngineeringOffice) || User.DistrictEngineeringOffice == "Select")
        {
            OnDistrict = false;
        }
    }
}

