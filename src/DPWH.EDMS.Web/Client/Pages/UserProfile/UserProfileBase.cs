using AutoMapper;
using Blazored.Toast.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Extensions;
using DPWH.EDMS.Shared.Enums;
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.UserProfile;

public class UserProfileBase : RxBaseComponent
{
    protected UserModel CurrentUser { get; set; } = new();
    protected string Role = string.Empty;

    [Inject] public required IUsersService UserService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }
    [Inject] public required IDocumentService DocumentService { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    protected TelerikDialog dialogReference = new();

    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    protected List<string> AllowedExtensions { get; set; } = new List<string>() {".png"};
    protected int MinFileSize { get; set; } = 1024;
    protected int MaxFileSize { get; set; } = 4 * 1024 * 1024;
    protected string? SignatureValidation { get; set; }
    protected bool IsSignatureSelected { get; set; }
    protected string Office = string.Empty;
    protected string? UriSignature { get; set; }

    //Upload Signature
    protected UploadSignatureImageModel? SelectedSignature { get; set; }



    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "person",
            Text = "Profile",
            Url = "/profile"
        });
    }

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        await GetUser();
        IsLoading = false;
    }
    private async Task GetUser()
    {
        if (AuthenticationStateAsync is null)
            return;

        var authState = await AuthenticationStateAsync;
        var user = authState.User;
        if (user.Identity is not null && user.Identity.IsAuthenticated)
        {
            var roles = user.Claims.Where(c => c.Type == "role")!.ToList();
            var role = roles.FirstOrDefault(role => !string.IsNullOrEmpty(role.Value) && role.Value.Contains(ApplicationRoles.RolePrefix))?.Value ?? string.Empty;

            var userId = user.Claims.FirstOrDefault(c => c.Type == "sub")!.Value;

            var userRes = await UserService.GetById(Guid.Parse(userId));
            Role = GetRoleLabel(role);

            if (userRes.Success)
            {
                CurrentUser = Mapper.Map<UserModel>(userRes.Data);
            }

            var office = ClaimsPrincipalExtensions.GetOffice(user);
            Office = !string.IsNullOrEmpty(office) ? office : "---";
            await GetUriSignature();
        }
    }
    private string GetRoleLabel(string role)
    {
        return ApplicationRoles.GetDisplayRoleName(role, "Unknown Role");
    }

    protected async Task OnSignature()
    {
        IsLoading = true;
        IsSignatureSelected = false;
    }
    protected void ValidateSignatureSelect()
    {
        SignatureValidation = string.Empty;

        if (SelectedSignature != null)
        {
            IsSignatureSelected = true;
        }
        else
        {
            SignatureValidation = "Select File to Upload";
        }
    }

    protected async Task OnSubmitUpload()
    {
        IsLoading = true;
        if (SelectedSignature?.Document != null)
        {
            await UserService.UploadSignature(SelectedSignature.Document);
            ToastService.ShowSuccess("Signature Successfully Uploaded!");
            await GetUriSignature();
            SelectedSignature = null;
            dialogReference.Refresh();
        }
        else
        {
            ValidateSignatureSelect();
        }

        StateHasChanged();
        IsLoading = false;
    }

    protected async void OnSelectSignature(FileSelectEventArgs args)
    {
        SelectedSignature = new UploadSignatureImageModel()
        {
            Document = null!
        };

        SelectedSignature.Document = await DocumentService.GetFileToUpload(args);
        SignatureValidation = string.Empty;

        StateHasChanged();
    }
    private async Task GetUriSignature()
    {
        try
        {
            var uri = await UserService.GetUserSignature();
            if (uri != null && uri.Data != null)
            {
                UriSignature = uri.Data.UriSignature;
                StateHasChanged();
            }
        } catch (Exception ex) 
        {
            // catch if no signature yet uploaded
            UriSignature = string.Empty; 

        }
        
        
    }
}

