using DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using DPWH.EDMS.Api.Contracts;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Telerik.FontIcons;
using DPWH.EDMS.Web.Client.Pages.UserManagement.Model;
using Telerik.Blazor.Components;
using DPWH.EDMS.Client.Shared.APIClient.Services.DpwhIntegrations;

namespace DPWH.EDMS.Web.Client.Pages.UserManagement;

public class UserManagementBase : GridBase<UserModel>
{
    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required ILicensesService LicensesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IDpwhIntegrationsService DpwhIntegrationService { get; set; }

    protected double LicenseLimit = 0;
    protected double LicenseUsed = 0;
    protected double TotalUsers = 0;
    protected bool DialogVisible { get; set; } = false;
    protected bool OnEmpID { get; set; } = true;
    protected bool OnSearchPIS { get; set; } = true;
    protected string getOpenbtn = "";
    public string ModalWith = "600px";
    public string SelectedRegionalOffice { get; set; }
    public string SelectedImplementingOffice { get; set; }
    public string UserRole { get; set; }

    protected string SelectedAcord { get; set; }
    protected UserManagementModel _user { get; set; } = new();
    protected UserManagementModel userModel { get; set; }
    protected TelerikDialog dialogReference = new();
    protected TelerikForm FormRef { get; set; } = new();

    protected ICollection<GetRequestingOfficeResult> RegionOfficeList { get; set; } = new List<GetRequestingOfficeResult>();
    protected List<GetRequestingOfficeResultItem> DEOlist { get; set; } = new List<GetRequestingOfficeResultItem>();

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "group",
            Text = "User Management",
            Url = "/user-management"
        });
    }

    protected override async Task OnInitializedAsync()
    {
        ServiceCb = UserService.Query;
        await LoadData();

        await ExceptionHandlerService.HandleApiException(
            async () =>
            {
                var licenseRes = await LicensesService.GetLicenseStatus();

                if (licenseRes.Success)
                {
                    var licenseData = licenseRes.Data;
                    //LicenseUsed = licenseData.Limit - licenseData.Available;
                    LicenseUsed = 1; 
                    LicenseLimit = licenseData.Limit;
                    TotalUsers = licenseData.EndUsersCount;
                }
            });
    } 
    protected double GetLicenseAccumulatedPercentage()
    {
        return Math.Round((LicenseUsed / LicenseLimit) * 100, 2);
    }

    protected async Task AddUser()
    {
        _user.Position = string.Empty;
        _user.FullName = string.Empty;
        _user.DesignationTitle = string.Empty;
        _user.RegionalOffice = "Select";
        _user.DistrictEngineeringOffice = "Select";

        _user.FirstName = string.Empty;
        _user.LastName = string.Empty;
        _user.Role = "Select";

        _user.Email = string.Empty;
        DialogVisible = true; getOpenbtn = "Add";
        SelectedAcord = "add";
        dialogReference.Refresh();
    }
    protected void ClearSearch()
    {
        _user.EmployeeId = null; 

        AddUser();
    }
    protected async Task OnSearchEmployeeID(string id)
    {
        IsLoading = true;
        if (id != null)
        {
            try
            {
                var res = await DpwhIntegrationService.GetByEmployeeId(id);
                if (res.Success)
                {
                    userModel = new UserManagementModel();
                    userModel.EmployeeId = id;
                    userModel.FirstName = res.Data.FirstName;
                    userModel.MiddleName = res.Data.MiddleInitial;
                    userModel.LastName = res.Data.FamilyName;
                    userModel.Email = res.Data.NetworkId + "@dpwh.gov.ph";
                    userModel.Position = res.Data.PlantillaPosition;
                    userModel.DesignationTitle = res.Data.DesignationTitle;

                }
                else
                {
                    ToastService.ShowError(id + " not found");
                }

                var fullName = userModel.LastName + ", " + userModel.FirstName + " " + userModel.MiddleName;
                _user.FullName = fullName;

                SelectedAcord = "add";
                _user.FirstName = userModel.FirstName;
                _user.MiddleName = userModel.MiddleName;
                _user.LastName = userModel.LastName;
                _user.Email = userModel.Email;
                _user.Position = userModel.Position;
                _user.DesignationTitle = userModel.DesignationTitle;
                ClearNotif();
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    //ToastService.ShowError(id + " not found");
                    OnEmpID = false;
                    OnSearchPIS = true;
                    ClearSearch();
                }
                else if (ex.StatusCode == 500)
                {
                    OnSearchPIS = false;
                    OnEmpID = true;
                    await OnSearchEmployeeID(id);
                }
                else
                {
                    await OnSearchEmployeeID(id);
                }
            }

        }
        else
        {
            SelectedAcord = "add";
            _user.FirstName = string.Empty;
            _user.LastName = string.Empty;
            _user.FullName = string.Empty;
            _user.Position = string.Empty;
            _user.DesignationTitle = string.Empty;

            _user.Role = "Select";

            _user.Email = string.Empty;
            DialogVisible = true; getOpenbtn = "Add";
        }
        IsLoading = false;
        dialogReference.Refresh(); 
    }
    protected void ClearNotif()
    {
        OnEmpID = true;
        OnSearchPIS = true;
    }
    //protected async Task OnRegionOfficeChanged()
    //{

    //    if (RegionOfficeList != null && SelectedRegionalOffice != null)
    //    {
    //        try
    //        {
    //            var selectedRegion = RegionOfficeList.FirstOrDefault(item => item.RegionName == SelectedRegionalOffice);

    //            if (selectedRegion != null)
    //            {
    //                SelectedRegionalOffice = selectedRegion.RegionName;
    //                _user.RegionalOffice = SelectedRegionalOffice;
    //                ResetDropDownOnOfficeChanged();
    //                DEOlist = selectedRegion.ImplementingOffices.ToList();
    //                OnRegion = true;
    //            }
    //            else
    //            { OnRegion = false; }
    //        }
    //        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
    //        {
    //            var problemDetails = apiExtension.Result;
    //            var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
    //            ToastService.ShowError(error);
    //        }
    //    }
    //    else
    //    {
    //        OnRegion = false;
    //        ResetDropDownOnOfficeChanged();
    //    }
    //    dialogReference.Refresh();
    //    ImplementRef.Refresh();

    //}
    private void ResetDropDownOnOfficeChanged()
    {
        DEOlist = new List<GetRequestingOfficeResultItem>();
    }

}
