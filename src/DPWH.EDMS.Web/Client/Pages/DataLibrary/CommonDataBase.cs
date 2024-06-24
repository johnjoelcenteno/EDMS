using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.DataLibrary;

public class CommonDataBase : GridBase<DataManagementModel>
{
    [Parameter] public string Id { get; set; }
    [Inject] public required IDataLibraryService _DataLibraryService { get; set; }
    [Inject] public required IToastService _ToastService { get; set; }

    protected string UriName { get; set; }
    protected string DataType { get; set; }
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
        FindDataLibraries(Id);

        BreadcrumbItems = new List<BreadcrumbModel>
        {
            new() { Icon = "home", Url = "/"},
            new() { Text = "Data Library", Url = "/data-lirary"},
            new() { Text = UriName, Url = $"/data-library/{Id}" },

        };
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadLibraryData();
        GetGridMenuItems();
    }

    protected void FindDataLibraries(string Id)
    {
        switch (Id)
        {
            case "record-types":
                UriName = "Record Types";
                DataType = "RecordTypes";
                break;

            case "valid-ids":
                UriName = "Valid IDs";
                DataType = "ValidIDs";
                break;

            case "authorization-documents":
                UriName = "Authorization Documents";
                DataType = "AuthorizationDocuments";
                break;

            default:
                break;
        }
    }

    protected virtual async Task LoadLibraryData()
    {
        IsLoading = true;
        FindDataLibraries(Id);
        try
        {
            var dataLibraryResults = await _DataLibraryService.GetDataLibraries();
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
        }
        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
        {
            var problemDetails = apiExtension.Result;
            var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
            _ToastService.ShowError(error);
        }

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
        try
        {
            var res = await _DataLibraryService.DeleteDataLibraries(Guid.Parse(id));

            if (res.Success)
            {
                _ToastService.ShowSuccess($"{NewConfig.Value} Successfully Deleted!");
            }
        }
        catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
        {
            var problemDetails = apiExtension.Result;
            var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
            _ToastService.ShowError(error);
        }

        await LoadLibraryData();
        IsLoading = false;
    }

    protected async Task OnUpdateItem(string id, ConfigModel item)
    {
        IsOpen = false;
        IsLoading = true;

        var newitem = new UpdateDataLibraryCommand()
        {
            Id = Guid.Parse(id),
            Value = item.Value,
        };

        if (id != null)
        {
            try
            {
                var res = await _DataLibraryService.UpdateDataLibraries(newitem);
                if (res.Success)
                {
                    _ToastService.ShowSuccess($"{newitem.Value} Successfully Updated!");

                }
            }
            catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
            {
                var problemDetails = apiExtension.Result;
                var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
                _ToastService.ShowError(error);
            }
        }

        await LoadLibraryData();
        IsLoading = false;
    }

    protected async Task OnSave(ConfigModel model)
    {
        IsLoading = true;
        var valid = FormRef.IsValid();

        if (string.IsNullOrEmpty(model.Value))
        {
            IsLoading = false;
            return;
        }
        else
        {
            if (valid)
            {
                IsOpen = false;
                if (Enum.TryParse<DataLibraryTypes>(DataType, out DataLibraryTypes dataTypeEnum))
                {
                    var data = new AddDataLibraryCommand
                    {
                        Type = dataTypeEnum,
                        Value = model.Value
                    };
                    try
                    {
                        var res = await _DataLibraryService.AddDataLibraries(data);

                        if (res.Success)
                        {
                            _ToastService.ShowSuccess($"{data.Value} Successfully Saved!");
                        }
                    }
                    catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
                    {
                        var problemDetails = apiExtension.Result;
                        var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
                        _ToastService.ShowError(error);
                    }

                }
            }
        }

        IsLoading = false;
        await LoadLibraryData();
    }
}
