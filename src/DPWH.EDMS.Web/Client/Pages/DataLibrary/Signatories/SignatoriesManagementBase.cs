
using AutoMapper;
using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordManagement;
using DPWH.EDMS.Client.Shared.APIClient.Services.Signatories;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;

namespace DPWH.EDMS.Web.Client.Pages.DataLibrary.Signatories;

public class SignatoriesManagementBase : GridBase<SignatoriesModel>
{
    [Inject] public required ISignatoryManagementService SignatoryService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }
    public bool checkRecordType = false;
    protected string UriName = "Signatories";
    protected string DataType = "Signatories";
    protected string SelectedAcord { get; set; }
    protected string ItemId { get; set; }
    protected bool IsOpen { get; set; } = false;
    protected bool IsConfirm { get; set; } = false;
    protected DataSourceResult GetSignatoryResult { get; set; } = new DataSourceResult();
    protected DataSourceRequest GetSignatory { get; set; } = new DataSourceRequest
    {
        Take = 0,
        Skip = 0
    };

    protected TelerikDialog dialogReference = new();
    protected TelerikForm FormRef { get; set; } = new TelerikForm();
    protected SignatoryManagementModel NewConfig = new SignatoryManagementModel();
    protected SignatoriesModel SelectedItem { get; set; } = default!;
    protected TelerikContextMenu<GridMenuItemModel> ContextMenuRef { get; set; } = new();
    protected List<GridMenuItemModel> MenuItems { get; set; } = new();
    protected string GetOpenbtn { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {

        BreadcrumbItems = new List<BreadcrumbModel>
        {
            new() { Icon = "home", Url = "/"},
            new() { Text = "Data Library", Url = "/data-library"},
            new() { Text = "Signatories", Url = $"/data-library/signatories" },

        };
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadLibraryData();
        GetGridMenuItems();
    }


    protected virtual async Task LoadLibraryData()
    {
        IsLoading = true;

        await ExceptionHandlerService.HandleApiException(async () =>
        {

            var signatoriesResults = await SignatoryService.Query(GetSignatory);

            if (signatoriesResults.Data != null)
            {
                var checkIsActive = GenericHelper.GetListByDataSource<SignatoriesModel>(signatoriesResults.Data);
               var convertData =  checkIsActive.Where(item => item.IsActive)
                .Select(item => new SignatoriesModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    DocumentType = item.DocumentType,
                    Position = item.Position,
                    Office1 = item.Office1,
                    Office2 = item.Office2,
                    SignatoryNo = item.SignatoryNo,
                    IsActive = item.IsActive
                }) .ToList();
                GridData = convertData;
                
            }
        });




        IsLoading = false;
    }

    protected void GetGridMenuItems()
    {
        MenuItems = new List<GridMenuItemModel>()
            {
                new (){ Text = "Edit", Icon=null!, CommandName="Edit" },
                new (){ Text = "Delete", Icon=null!, CommandName="Delete" },
            };
    }

    protected async Task ShowRowOptions(MouseEventArgs e, SignatoriesModel row)
    {
        SelectedItem = row;
        ItemId = row.Id.ToString();
        await ContextMenuRef.ShowAsync(e.ClientX, e.ClientY);
    }

    protected void OnItemClick(GridMenuItemModel item)
    {
        if (item.Action != null)
        {
            item.Action.Invoke();
        }
        else
        {
            SelectedAcord = item.CommandName;

            switch (item.CommandName)
            {

                case "Edit":
                    GetOpenbtn = "Edit";

                    NewConfig.Name = SelectedItem.Name;
                    NewConfig.DocumentType = SelectedItem.DocumentType;
                    NewConfig.Position = SelectedItem.Position;
                    NewConfig.Office1 = SelectedItem.Office1;
                    NewConfig.Office2 = SelectedItem.Office2;
                    NewConfig.SignatoryNo = SelectedItem.SignatoryNo;
                    NewConfig.Id = SelectedItem.Id;

                    IsOpen = true;
                    dialogReference.Refresh();
                    break;

                case "Add":
                    GetOpenbtn = "Add";
                    break;

                case "Delete":
                    if (SelectedItem != null)
                    {
                        NewConfig.Name = SelectedItem.Name;
                        NewConfig.DocumentType = SelectedItem.DocumentType;
                        NewConfig.Position = SelectedItem.Position;
                        NewConfig.Office1 = SelectedItem.Office1;
                        NewConfig.Office2 = SelectedItem.Office2;
                        NewConfig.SignatoryNo = SelectedItem.SignatoryNo;
                        NewConfig.Id = SelectedItem.Id;
                    }
                    IsConfirm = true;
                    dialogReference.Refresh();
                    break;

                default:
                    break;
            }
        }

        SelectedItem = null!;
    }

    

    protected async Task OnDeleteItem(string id)
    {
        IsConfirm = false;
        IsLoading = true;

        await ExceptionHandlerService.HandleApiException(async () =>
        {
            var res = await SignatoryService.DeleteSignatoriesAsync(Guid.Parse(id));
        }, null, $"{NewConfig.Name} Successfully Deleted!");

        await LoadLibraryData();
        IsLoading = false;
    }

    protected async Task OnUpdateItem(SignatoryManagementModel item)
    {
        IsOpen = false;
        IsLoading = true;
        if (string.IsNullOrEmpty(item.Name))
        {
            IsLoading = false;
            return;
        }
        else
        {
            var newitem = new UpdateSignatoryModel()
            {
                Name = item.Name,
                DocumentType = item.DocumentType,
                Position = item.Position,
                Office1 = item.Office1,
                Office2 = item.Office2,
                SignatoryNo = item.SignatoryNo,
                IsActive = true
            };


            await ExceptionHandlerService.HandleApiException(async () =>
            {
                var res = await SignatoryService.UpdateSignatoriesAsync(item.Id, newitem);
                if (res.Success)
                {
                    IsOpen = false;
                }
            }, null, $"{newitem.Name} Successfully Updated!");

            await LoadLibraryData();
            IsLoading = false;
        }
        
    }
    protected void OnCancel()
    {
        IsOpen = false;
    }
    protected void HandleCancel(string uri)
    {
        NavManager.NavigateTo(uri);
    }
    protected async Task OnSave(SignatoryManagementModel model)
    {
        IsLoading = true;
        if (string.IsNullOrEmpty(model.Name))
        {
            IsLoading = false;
            return;
        }
        else
        {

            IsOpen = false;
            var data = new CreateSignatoryModel
            {
                IsActive = true,
                Name = model.Name,
                DocumentType = model.DocumentType,
                Position = model.Position,
                Office1 = model.Office1,
                Office2 = model.Office2,
                SignatoryNo = model.SignatoryNo
            };

            await ExceptionHandlerService.HandleApiException(async () =>
            {
                var res = await SignatoryService.Create(data);

            }, null, $"{data.Name} Successfully Saved!");



        }
        IsLoading = false;
        await LoadLibraryData();
    }
}