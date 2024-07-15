﻿
using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;

namespace DPWH.EDMS.Web.Client.Pages.DataLibrary.DataLibraries.ValidIds;

public class ValidIdsManagementBase : GridBase<DataManagementModel>
{
    [Inject] public required IDataLibraryService DataLibraryService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }
    public bool checkRecordType = false;
    protected string UriName = "Valid Ids";
    protected string DataType = "ValidIDs";
    protected string SelectedAcord { get; set; }
    protected string getOpenbtn = "";
    protected string ItemId { get; set; }
    protected bool IsOpen { get; set; } = false;
    protected bool IsConfirm { get; set; } = false;

    protected GetDataLibraryResult GetDataLibrary { get; set; } = new GetDataLibraryResult();

    protected TelerikDialog dialogReference = new();
    protected TelerikForm FormRef { get; set; } = new TelerikForm();
    protected ConfigModel NewConfig = new ConfigModel();
    protected GetDataLibraryResultValue SelectedItem { get; set; } = default!;
    protected TelerikContextMenu<GridMenuItemModel> ContextMenuRef { get; set; } = new();
    protected List<GridMenuItemModel> MenuItems { get; set; } = new();

    protected override void OnParametersSet()
    {

        BreadcrumbItems = new List<BreadcrumbModel>
        {
            new() { Icon = "home", Url = "/"},
            new() { Text = "Data Library", Url = "/data-library"},
            new() { Text = "Valid Ids", Url = $"/data-library/valid-ids" },

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
            var dataLibraryResults = await DataLibraryService.GetDataLibraries();
            var propertyConditionData = dataLibraryResults.Data.FirstOrDefault(item => item.Type == DataType);

            if (propertyConditionData != null)
            {
                var convertedData = propertyConditionData.Data.Where(item => !item.IsDeleted)
                    .Select(item => new GetDataLibraryResultValue
                    {
                        Id = item.Id,
                        Value = item.Value,
                        Created = item.Created,
                        CreatedBy = item.CreatedBy,
                        IsDeleted = item.IsDeleted
                    })
                    .ToList();

                GetDataLibrary = new GetDataLibraryResult
                {
                    Type = propertyConditionData.Type,
                    Data = convertedData
                };
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

    protected async Task ShowRowOptions(MouseEventArgs e, GetDataLibraryResultValue row)
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
                    getOpenbtn = "Edit";

                    NewConfig.Value = SelectedItem.Value;
                    NewConfig.Id = SelectedItem.Id.ToString();

                    IsOpen = true;
                    dialogReference.Refresh();
                    break;

                case "Add":
                    getOpenbtn = "Add";
                    dialogReference.Refresh();
                    break;

                case "Delete":
                    NewConfig.Value = SelectedItem.Value;

                    IsConfirm = true;
                    dialogReference.Refresh();
                    break;

                default:
                    break;
            }
        }

        SelectedItem = null!;
    }

    protected async Task AddUser()
    {
        SelectedAcord = "Add";
        NewConfig.Value = string.Empty;
        getOpenbtn = "Add";
        IsOpen = true;
        FormRef.Refresh();
        dialogReference.Refresh();
    }

    protected async Task OnDeleteItem(string id)
    {
        IsConfirm = false;
        IsLoading = true;

        await ExceptionHandlerService.HandleApiException(async () =>
        {
            var res = await DataLibraryService.DeleteDataLibraries(Guid.Parse(id));
        }, null, $"{NewConfig.Value} Successfully Deleted!");

        await LoadLibraryData();
        IsLoading = false;
    }

    protected async Task OnUpdateItem(ConfigModel item)
    {
        IsOpen = false;
        IsLoading = true;
        if (string.IsNullOrEmpty(item.Value))
        {
            IsLoading = false;
            return;
        }

        var newitem = new UpdateDataLibraryCommand()
        {
            Id = Guid.Parse(item.Id),
            Value = item.Value,
        };

        if (item.Id != null)
        {

            await ExceptionHandlerService.HandleApiException(async () =>
            {
                var res = await DataLibraryService.UpdateDataLibraries(newitem);
                if (res.Success)
                {
                    IsOpen = false;
                }
            }, null, $"{newitem.Value} Successfully Updated!");
        }

        await LoadLibraryData();
        IsLoading = false;
    }
    protected void OnCancel()
    {
        IsOpen = false;
    }
    protected void HandleCancel(string uri)
    {
        NavManager.NavigateTo(uri);
    }
    protected async Task OnSave(ConfigModel model)
    {
        IsLoading = true;
        if (string.IsNullOrEmpty(model.Value))
        {
            IsLoading = false;
            return;
        }
        else
        {

            if (Enum.TryParse<DataLibraryTypes>(DataType, out DataLibraryTypes dataTypeEnum))
            {
                IsOpen = false;
                var data = new AddDataLibraryCommand
                {
                    Type = dataTypeEnum,
                    Value = model.Value
                };

                await ExceptionHandlerService.HandleApiException(async () =>
                {
                    var res = await DataLibraryService.AddDataLibraries(data);

                }, null, $"{data.Value} Successfully Saved!");
            }


        }
        IsLoading = false;
        await LoadLibraryData();
    }
}