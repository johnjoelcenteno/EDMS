using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.Common.Model;
using DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.EmployeeRecords;

public class EmployeeRecordsManagementBase : RecordTypesFormComponentBase
{


    #region Boolean Declaration

    protected bool IsOpen { get; set; } = false;
    protected bool IsConfirm { get; set; } = false;
    #endregion


    #region Telerik Grid Declaration

    protected TelerikContextMenu<GridMenuItemModel> ContextMenuRef { get; set; } = new();
    protected List<GridMenuItemModel> MenuItems { get; set; } = new();
    protected string SelectedAcord { get; set; }

    protected List<RecordsLibraryModel> GetRecordType = new List<RecordsLibraryModel>();

    protected int Page { get; set; } = 1;
    protected int PageSize { get; set; } = 5;
    protected int PageMapSize { get; set; } = 3;
    protected string GridHeight { get; set; } = "auto";
    protected List<int?> PageSizes { get; set; } = new List<int?> { 5, 10, 15 };

    protected RecordsLibraryModel SelectedItem { get; set; } = default!;
    protected RecordsLibraryModel SelectedRecordItem { get; set; } = default!;

    protected string GetOpenbtn { get; set; } = string.Empty;


    #endregion


    protected override async Task OnParametersSetAsync()
    {

        BreadcrumbItems = new List<BreadcrumbModel>
        {
            new() { Icon = "home", Url = "/"},
            new() { Text = "Data Library", Url = "/data-library"},
            new() { Text = "Employee Records", Url ="/data-library/employee-records" },

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
            var dataLibraryResults = await RecordTypesService.QueryByCategoryRecordTypesAsync("Employee Documents");
            if (dataLibraryResults.Success)
            {
                var convertedData = dataLibraryResults.Data.Where(item => item.IsActive)
                    .Select(item => new RecordsLibraryModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Section = item.Section,
                        Category = "System", //temporary until CreatedBy is created
                        Office = item.Office,
                        IsActive = !item.IsActive,
                    }).ToList();
                GetRecordType = convertedData;
            }
        });
        IsLoading = false;
    }


    protected async void OnItemClick(GridMenuItemModel item)
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
                    IsOpen = true;
                    GetOpenbtn = "Edit";
                    if (SelectedItem != null)
                    {
                        NewConfig.Name = SelectedItem.Name;
                        NewConfig.Id = SelectedItem.Id;
                        NewConfig.Category = SelectedItem.Category;
                        NewConfig.Section = SelectedItem.Section;
                        NewConfig.Office = SelectedItem.Office;
                    }

                    DialogReference.Refresh();
                    StateHasChanged();
                    break;

                case "Add":
                    GetOpenbtn = "Add";
                    break;

                case "Delete":
                    if (SelectedItem != null)
                    {
                        NewConfig.Name = SelectedItem.Name;
                        NewConfig.Id = SelectedItem.Id;
                        NewConfig.Category = SelectedItem.Category;
                        NewConfig.Section = SelectedItem.Section;
                        NewConfig.Office = SelectedItem.Office;
                    }
                    IsConfirm = true;
                    DialogReference.Refresh();
                    break;

                default:
                    break;
            }
        }
        SelectedItem = null!;
    }

    protected async Task ShowRowOptions(MouseEventArgs e, RecordsLibraryModel row)
    {
        SelectedItem = row;
        await ContextMenuRef.ShowAsync(e.ClientX, e.ClientY);
    }

    protected async Task ShowRecordRowOptions(MouseEventArgs e, RecordsLibraryModel row)
    {
        SelectedItem = row;
        await ContextMenuRef.ShowAsync(e.ClientX, e.ClientY);
    }
    protected void GetGridMenuItems()
    {
        MenuItems = new List<GridMenuItemModel>()
            {
                new (){ Text = "Edit", Icon=null!, CommandName="Edit" },
                new (){ Text = "Delete", Icon=null!, CommandName="Delete" },
            };
    }

    protected void OnCancel()
    {
        IsOpen = false;
    }

    protected async Task OnSave(RecordsLibraryModel model)
    {
        IsLoading = true;
        if (string.IsNullOrEmpty(model.Name))
        {
            IsLoading = false;

            IsSectionEmpty = string.IsNullOrEmpty(model.Section);
            IsOfficeEmpty = string.IsNullOrEmpty(model.Office);

            return;
        }
        else
        {


            if (string.IsNullOrEmpty(model.Section) || string.IsNullOrEmpty(model.Office))
            {
                IsSectionEmpty = string.IsNullOrEmpty(model.Section);
                IsOfficeEmpty = string.IsNullOrEmpty(model.Office);
            }
            else
            {
                IsOpen = false;
                var data = new CreateRecordTypeModel
                {
                    IsActive = true,
                    Category = "Employee Documents",
                    Name = model.Name,
                    Section = model.Section,
                    Office = model.Office
                };
                await ExceptionHandlerService.HandleApiException(async () =>
                {

                    var res = await RecordTypesService.CreateRecordTypesAsync(data);
                }, null, $"{data.Name} Successfully Saved!");
            }



        }

        IsLoading = false;
        await LoadLibraryData();
    }
    protected async Task OnUpdateItem(RecordsLibraryModel item)
    {
        IsOpen = false;
        IsLoading = true;
        if (string.IsNullOrEmpty(item.Name))
        {
            IsLoading = false;
            IsSectionEmpty = string.IsNullOrEmpty(item.Section);
            IsOfficeEmpty = string.IsNullOrEmpty(item.Office);
            return;
        }


            if (string.IsNullOrEmpty(item.Section) || string.IsNullOrEmpty(item.Office))
            {
                IsSectionEmpty = string.IsNullOrEmpty(item.Section);
                IsOfficeEmpty = string.IsNullOrEmpty(item.Office);

            }
            else
            {
                var data = new UpdateRecordTypeModel
                {
                    IsActive = true,
                    Category = "Employee Documents",
                    Name = item.Name,
                    Section = item.Section,
                    Office = item.Office
                };
                await ExceptionHandlerService.HandleApiException(async () =>
                {
                    var res = await RecordTypesService.UpdateRecordTypesAsync(item.Id, data);
                    if (res != null)
                    {
                        IsOpen = false;
                    }

                }, null, $"{data.Name} Successfully Updated!");
            }

        await LoadLibraryData();
        IsLoading = false;
    }

    protected async Task OnDeleteItem(string id)
    {
        IsConfirm = false;
        IsLoading = true;

            await ExceptionHandlerService.HandleApiException(async () =>
            {
                var res = await RecordTypesService.DeleteRecordTypesAsync(Guid.Parse(id));
            }, null, $"{NewConfig.Name} Successfully Deleted!");
        await LoadLibraryData();
        IsLoading = false;
    }

}

