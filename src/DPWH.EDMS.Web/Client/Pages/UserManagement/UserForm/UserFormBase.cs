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
using DPWH.EDMS.IDP.Core.Constants;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DPWH.EDMS.Web.Client.Pages.UserManagement.UserForm;

public class UserFormBase : RxBaseComponent
{
    [Inject] public required ILookupsService LookupService { get; set; }
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required ILicensesService LicensesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IDpwhIntegrationsService DpwhIntegrationService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    //[Inject] public required IAddressManagerService AddressManagerService { get; set; }
    [Parameter] public EventCallback HandleCreateOnSubmit { get; set; }
    [Parameter] public EventCallback HandleOnCancel { get; set; }
    protected UserManagementModel User { get; set; } = new();
    protected UserManagementModel UserModel { get; set; }
    protected TelerikForm FormRef { get; set; } = new();

    public string SelectedRegionalOffice { get; set; }
    public string SelectedImplementingOffice { get; set; }
    protected string SelectedAcord { get; set; }
    protected string UserRole { get; set; }
    protected string Role { get; set; }
    protected string RegionOffice { get; set; }
    protected string DistrictOffice { get; set; }
    protected string CentralOffice = "Central Office";
    protected string SelectedOffice { get; set; }
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


    protected override async Task OnInitializedAsync()
    {
        SelectedAcord = "add";

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

        User.Email = string.Empty;
        SelectedAcord = "add";
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
            }, null, null, true, async (bool empId, bool pisSearch) => await PISException(empId, pisSearch)
            );

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

    protected async Task PISException(bool empId, bool searchPIS)
    {
        // status code 500
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
            OnRegion = false;
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

        }
        if (User.Role == ApplicationRoles.Manager)
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
    protected async Task AddUser(UserManagementModel user)
    {
        IsLoading = true;
        //validateRegion();
        if (OnRegion != false && OnDistrict != false)
        {
            if (User.Role != "Select")
            {

                if (User.RegionalOffice == null)
                {
                    user.RegionalOffice = string.Empty;
                }
                else
                {
                    user.RegionalOffice = User.RegionalOffice;
                }
                if (User.DistrictEngineeringOffice == null)
                {
                    user.DistrictEngineeringOffice = string.Empty;
                }
                else
                {
                    user.DistrictEngineeringOffice = User.DistrictEngineeringOffice;
                }

                IsSaving = true;

                if (LicenseLimit <= 0 && User.Role != "dpwh_requestor" && User.Role != "dpwh_inspector")
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
                    Position = user.Position
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
}

