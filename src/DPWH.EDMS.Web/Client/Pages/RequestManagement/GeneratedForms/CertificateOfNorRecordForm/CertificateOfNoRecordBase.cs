using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Web.Client.Shared.Services.Drawing;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Client.Shared.APIClient.Services.Signatories;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.IDP.Core.Constants;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.GeneratedForms.CertificateOfNorRecordForm;

public class CertificateOfNoRecordBase : GridBase<SignatoryModel>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Parameter] public required string RequestId { get; set; }
    [Inject] public required IRequestManagementService RequestManagementService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; } 
    [Inject] public required IUsersService UsersService { get; set; }
    [Inject] public required ILookupsService LookupsService { get; set; }
    [Inject] public required ISignatoryManagementService SignatoryManagementService { get; set; }
    [Inject] public required NavigationManager Navigate { get; set; }
    [Inject] protected DrawingService drawingService { get; set; } = default!;
    protected bool IsLoading { get; set; } = false;
    [Parameter] public required string IsExporting { get; set; }
    protected RecordRequestModel SelectedRecordRequest { get; set; } = new();
    protected GetUserByIdResult User = new GetUserByIdResult();
    protected SignatoryModel SignatoryData { get; set; }
    protected ElementReference CurrentPdfContainerRef { get; set; }
    protected ElementReference NonCurrentPdfContainerRef { get; set; }
    protected string DisplayName = "---";
    protected string Role = string.Empty;
    protected string Office = string.Empty;
    protected bool IsCurrentSection { get; set; } = false;
    protected bool IsNonCurrentSection { get; set; } = false;
    protected string CurrentSectionSignature { get; set; }
    protected string NonCurrentSectionSignature { get; set; }
    protected string NotedSignature { get; set; }
    protected async override Task OnInitializedAsync()
    {

        IsLoading = true;
        await FetchUser();

        await LoadData(async (res) =>
        {
            SelectedRecordRequest = res;

            if(SelectedRecordRequest.RequestedRecords != null && SelectedRecordRequest.RequestedRecords.Count > 0)
{
                var office = !string.IsNullOrEmpty(User.Office) ? User.Office : "";
                var norecords = SelectedRecordRequest.RequestedRecords.Where(x => x.Uri == null).ToList();

                foreach (var item in norecords)
                {
                    if (item.Office == office)
                    {
                        ValidateSection(item.CategoryType);

                    }
                }
            }
             
        });
        await GetSignatories();
        await InvokeAsync(StateHasChanged);

        IsLoading = false;
    }

    protected async Task LoadData(Action<RecordRequestModel> onLoadCb)
    {
        await ExceptionHandlerService.HandleApiException(async () => {
            var recordReq = await RequestManagementService.GetById(Guid.Parse(RequestId));

            if (recordReq.Success)
            {
                if (onLoadCb != null)
                {
                    onLoadCb.Invoke(recordReq.Data);
                }
            }
            else
            {
                ToastService.ShowError("Something went wrong on loading record request.");
            }
        });
    }

    private async Task GetSignatories()
    {
        IsLoading = true;

        await ExceptionHandlerService.HandleApiException(async () =>
        {
            ServiceCb = SignatoryManagementService.Query;

            var filters = new List<Filter>();
            AddTextSearchFilter(filters, nameof(SignatoriesModel.DocumentType), "Certificate of no record", "contains");
           
            SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
            SearchFilterRequest.Filters = filters.Any() ? filters : null;
            await LoadData();

        });

        await GetSignatures();

        IsLoading = false;
    }
    protected async Task ExportPdf()
    {
        IsLoading = true;

        if (SelectedRecordRequest?.RequestedRecords is null)
            return;

       if(IsCurrentSection && IsNonCurrentSection)
        {

            var references = new List<(ElementReference reference, string section)>
            {
                (CurrentPdfContainerRef, "Current Section"),
                (NonCurrentPdfContainerRef, "Non-Current Section")
            };

            foreach (var (reference, section) in references)
            {
                var options = new { padding = "0cm", margin = "0cm", paperSize = "A4", scale = 0.7, multiPage = true, landscape = false };
                var data = await drawingService.ExportPdf(reference, options);
                await drawingService.SaveAs(data, $"{section}-Certificate-Of-No-Records-{DateTime.Now:MMM dd, yyyy}.pdf");
            }
        }
        
        IsLoading = false;
    }

    protected async Task GetSignatures()
    {
        var currentSection = GridData.FirstOrDefault(x => x.Office1 == "Current Section" && x.SignatoryNo == 0);
        var nonCurrentSection = GridData.FirstOrDefault(x => x.Office1 == "Non-Current Section" && x.SignatoryNo == 0);

        var notedSignature = GridData.FirstOrDefault(x => x.Office1 == "Office of the Chief" && x.SignatoryNo == 1);

        var tasks = new List<Task<GetUserProfileDocumentModelBaseApiResponse>>();

        if (currentSection != null && IsCurrentSection)
        {
            tasks.Add(UsersService.GetUserSignatureByEmployeeId(currentSection.EmployeeNumber));
        }

        if (nonCurrentSection != null && IsNonCurrentSection)
        {
            tasks.Add(UsersService.GetUserSignatureByEmployeeId(nonCurrentSection.EmployeeNumber));
        }

        if(notedSignature != null)
        {
            tasks.Add(UsersService.GetUserSignatureByEmployeeId(notedSignature.EmployeeNumber));
        }
        var results = await Task.WhenAll(tasks);
        var timestamp = DateTime.UtcNow.Ticks.ToString();
        foreach (var result in results)
        {
            if (result != null && result.Data != null)
            {
                if (currentSection != null && result.Data.EmployeeNumber == currentSection.EmployeeNumber)
                {
                    CurrentSectionSignature = $"{result.Data.UriSignature}?t={timestamp}";
                }
                if (nonCurrentSection != null && result.Data.EmployeeNumber == nonCurrentSection.EmployeeNumber)
                {
                    NonCurrentSectionSignature = $"{result.Data.UriSignature}?t={timestamp}";
                }
                if (notedSignature != null && result.Data.EmployeeNumber == notedSignature.EmployeeNumber)
                {
                    NotedSignature = $"{result.Data.UriSignature}?t={timestamp}";
                }
            }
        }
    }
    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(IsExporting))
        {
            if (IsExporting.Equals("exporting", StringComparison.OrdinalIgnoreCase))
            {
                await ExportPdf();
            }
        }
    }

    protected string GetDayWithSuffix(int day)
    {
        if (day >= 11 && day <= 13)
        {
            return day.ToString() + "th";
        }

        return (day % 10) switch
        {
            1 => day.ToString() + "st",
            2 => day.ToString() + "nd",
            3 => day.ToString() + "rd",
            _ => day.ToString() + "th",
        };
    }
    protected async Task GetAuthorizedStampSignatories(string employeeId)
    {
        //Ongoing integration
        //BE Ongoing
        var dataSource = new DataSourceRequest();
        var filters = new Api.Contracts.Filter
        {
            Field = nameof(SignatoryModel.EmployeeNumber),
            Operator = "eq",
            Value = employeeId
        };
        dataSource.Filter = filters;

        var getSignature = await SignatoryManagementService.Query(dataSource);
    
        if (getSignature != null)
        {
            var data = GenericHelper.GetListByDataSource<SignatoryModel>(getSignature.Data);
            if (data != null)
            {
                var userData = data.FirstOrDefault(x => x.EmployeeNumber == employeeId);
                if (userData != null)
                {
                    SignatoryData = userData;
                }
                //else
                //{
                //    ToastService.ShowError($"Employee ID :{employeeId} not found");
                //}
            }
        }
    }
    protected async Task FetchUser()
    {
        var authState = await AuthenticationStateAsync!;
        var user = authState.User;

        if (authState != null)
        {
            var roles = user.Claims.Where(c => c.Type == "role")!.ToList();

            var role = roles.FirstOrDefault(role => !string.IsNullOrEmpty(role.Value) && role.Value.Contains(ApplicationRoles.RolePrefix))?.Value ?? ClaimsPrincipalExtensions.GetRole(user);

            var firstnameValue = ClaimsPrincipalExtensions.GetFirstName(user);
            var lastnameValue = ClaimsPrincipalExtensions.GetLastName(user);
            var office = ClaimsPrincipalExtensions.GetOffice(user);
            var employeeId = ClaimsPrincipalExtensions.GetEmployeeNumber(user);

            DisplayName = (!string.IsNullOrEmpty(firstnameValue) && !string.IsNullOrEmpty(lastnameValue))
                ? GenericHelper.CapitalizeFirstLetter($"{firstnameValue} {lastnameValue}")
                : "---";
            if (employeeId != null)
            {
                await GetAuthorizedStampSignatories(employeeId);
            }
            User.Office = office;
            Role = GetRoleLabel(role);
        }
    }

    protected void ValidateSection(string section)
    {
        if (section == "Current Section")
        {
            IsCurrentSection = true;
        }
        else if (section == "Non-Current Section")
        {
            IsNonCurrentSection = true;
        }
    }
    protected string GetOfficeName(string officeCode)
    {
        return officeCode switch
        {
            nameof(Offices.RMD) => "Records Management Division",
            nameof(Offices.HRMD) => "Human Resource Management Division",
            _ => string.Empty
        };
    }
    private string GetRoleLabel(string roleValue)
    {
        return ApplicationRoles.GetDisplayRoleName(roleValue, "Unknown Role");
    }
    protected async Task<bool> GetCurrentSection(string DocumentType)
    {
        var data = await LookupsService.GetIssuances();
        if (data != null)
        {
            var Issuances = data.Data;
            return Issuances.Any(x => x.Name == DocumentType);
        }
        return false;
    }
    protected async Task<bool> GetNonCurrentSection(string DocumentType)
    {
        var data = await LookupsService.GetPersonalRecords();
        if (data != null)
        {
            var PersonalRecord = data.Data;
            return PersonalRecord.Any(x => x.Name == DocumentType);
        }
        return false;
    }
}