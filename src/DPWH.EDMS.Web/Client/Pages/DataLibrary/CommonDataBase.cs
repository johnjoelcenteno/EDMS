using Blazored.Toast.Services;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordTypes;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.Common.Enum;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using Telerik.SvgIcons;

namespace DPWH.EDMS.Web.Client.Pages.DataLibrary;

public class CommonDataBase : GridBase<DataManagementModel>
{
    [Parameter] public string Id { get; set; }
    [Inject] public required IDataLibraryService DataLibraryService { get; set; }
    [Inject] public required IRecordTypesService RecordTypesService { get; set; }
    [Inject] public required IExceptionHandlerService ExceptionHandlerService { get; set; }

    protected string UriName { get; set; }
    protected string DataType { get; set; }
    protected string SelectedAcord { get; set; }
    protected string getOpenbtn = "";
    protected string ItemId { get; set; }
    protected bool IsOpen { get; set; } = false;
    protected bool IsConfirm { get; set; } = false;

    protected bool IsSectionEmpty { get; set; } = false;
    protected bool IsOfficeEmpty { get; set; } = false;

    protected GetDataLibraryResult GetDataLibrary { get; set; } = new GetDataLibraryResult();

    protected TelerikDialog dialogReference = new();
    protected TelerikForm FormRef { get; set; } = new TelerikForm();
    protected ConfigModel NewConfig = new ConfigModel();
    protected GetDataLibraryResultValue SelectedItem { get; set; } = default!;
    protected TelerikContextMenu<GridMenuItemModel> ContextMenuRef { get; set; } = new();
    protected List<GridMenuItemModel> MenuItems { get; set; } = new();
    protected List<string> SectionList { get; set; } = new();
    protected List<string> OfficeList { get; set; } = new();
    protected List<QueryRecordTypesModel> QueryRecordTypesModels { get; set; } = new List<QueryRecordTypesModel>();

    protected override void OnParametersSet()
    {
        FindDataLibraries(Id);

        BreadcrumbItems = new List<BreadcrumbModel>
        {
            new() { Icon = "home", Url = "/"},
            new() { Text = "Data Library", Url = "/data-library"},
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
            case "personal-records":
                UriName = "Personal Records";
                DataType = DataLibraryEnum.PersonalRecords.ToString();
                break;

            case "valid-ids":
                UriName = "Valid IDs";
                DataType = DataLibraryEnum.ValidIDs.ToString();
                break;

            case "authorization-documents":
                UriName = "Authorization Documents";
                DataType = DataLibraryEnum.AuthorizationDocuments.ToString();
                break;
            case "purposes":
                UriName = "Purposes";
                DataType = DataLibraryEnum.Purposes.ToString();
                break;
            case "issuances":
                UriName = "DPWH Issuances";
                DataType = DataLibraryEnum.Issuances.ToString();
                break;
            case "employee-records":
                UriName = "Employee Records";
                DataType = DataLibraryEnum.EmployeeRecords.ToString();
                break;
            default:
                break;
        }
    }

    protected virtual async Task LoadLibraryData()
    {
        IsLoading = true;
        FindDataLibraries(Id);

        await ExceptionHandlerService.HandleApiException(async () =>
        {
            if (DataType == DataLibraryEnum.EmployeeRecords.ToString())
            {
                var dataLibraryResults = await RecordTypesService.QueryByCategoryRecordTypesAsync(UriName);
                if (dataLibraryResults.Success)
                {
                    QueryRecordTypesModels = dataLibraryResults.Data.ToList();
                    var convertedData = dataLibraryResults.Data.Where(item => item.IsActive)
                        .Select(item => new GetDataLibraryResultValue
                        {
                            Id = item.Id,
                            Value = item.Name,
                            Created = DateTime.Now,
                            CreatedBy = "system",
                            IsDeleted = !item.IsActive
                        }).ToList();
                    GetDataLibrary = new GetDataLibraryResult
                    {
                        Type = UriName,
                        Data = convertedData
                    };

                    SectionList = new List<string> {
                        "Employee Welfare and Benefits Section",
                        "Current Section",
                        "Non-Current Section"
                };
                    OfficeList = new List<string>
                {
                        "HRMD",
                        "RMD"
                };
                }
            }
            else
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
                    if (DataType == DataLibraryEnum.EmployeeRecords.ToString() && QueryRecordTypesModels.Count > 0)
                    {
                        var checkItem = QueryRecordTypesModels.FirstOrDefault(dt => dt.Name == SelectedItem.Value);
                        if (checkItem != null)
                        {
                            NewConfig.Section = checkItem.Section;
                            NewConfig.Office = checkItem.Office;
                        }
                    }


                    IsOpen = true;
                    dialogReference.Refresh();
                    break;

                case "Add":
                    getOpenbtn = "Add";
                    dialogReference.Refresh();
                    break;

                case "Delete":
                    NewConfig.Value = SelectedItem.Value;
                    if (DataType == DataLibraryEnum.EmployeeRecords.ToString() && QueryRecordTypesModels.Count > 0)
                    {
                        var checkItem = QueryRecordTypesModels.FirstOrDefault(dt => dt.Name == SelectedItem.Value);
                        if (checkItem != null)
                        {
                            NewConfig.Section = checkItem.Section;
                            NewConfig.Office = checkItem.Office;
                        }
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
        if (DataType == DataLibraryEnum.EmployeeRecords.ToString())
        {
            await ExceptionHandlerService.HandleApiException(async () =>
            {
                var res = await RecordTypesService.DeleteRecordTypesAsync(Guid.Parse(id));
            }, null, $"{NewConfig.Value} Successfully Deleted!");
        }
        else
        {
            await ExceptionHandlerService.HandleApiException(async () =>
            {
                var res = await DataLibraryService.DeleteDataLibraries(Guid.Parse(id));
            }, null, $"{NewConfig.Value} Successfully Deleted!");

            //try
            //{
            //    var res = await DataLibraryService.DeleteDataLibraries(Guid.Parse(id));

            //    if (res.Success)
            //    {
            //        ToastService.ShowSuccess($"{NewConfig.Value} Successfully Deleted!");
            //    }
            //}
            //catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
            //{
            //    var problemDetails = apiExtension.Result;
            //    var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
            //    ToastService.ShowError(error);
            //}
        }



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
            IsSectionEmpty = string.IsNullOrEmpty(item.Section);
            IsOfficeEmpty = string.IsNullOrEmpty(item.Office);
            return;
        }

        if (DataType == DataLibraryEnum.EmployeeRecords.ToString())
        {
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
                    Category = "Employee Records",
                    Name = item.Value,
                    Section = item.Section,
                    Office = item.Office
                };
                await ExceptionHandlerService.HandleApiException(async () =>
                {
                    var res = await RecordTypesService.UpdateRecordTypesAsync(Guid.Parse(item.Id), data);
                    if (res != null)
                    {
                        IsOpen = false;
                    }

                }, null, $"{data.Name} Successfully Updated!");
            }
        }
        else
        {
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
                //try
                //{
                //    var res = await DataLibraryService.UpdateDataLibraries(newitem);
                //    if (res.Success)
                //    {
                //        ToastService.ShowSuccess($"{newitem.Value} Successfully Updated!");

                //    }
                //}
                //catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
                //{
                //    var problemDetails = apiExtension.Result;
                //    var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
                //    ToastService.ShowError(error);
                //}
            }
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
            if (DataType == DataLibraryEnum.EmployeeRecords.ToString())
            {
                IsSectionEmpty = string.IsNullOrEmpty(model.Section);
                IsOfficeEmpty = string.IsNullOrEmpty(model.Office);
            }
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
                //try
                //{
                //    var res = await DataLibraryService.AddDataLibraries(data);

                //    if (res.Success)
                //    {
                //        ToastService.ShowSuccess($"{data.Value} Successfully Saved!");
                //        IsOpen = false;
                //    }
                //}
                //catch (Exception ex) when (ex is ApiException<ProblemDetails> apiExtension)
                //{
                //    var problemDetails = apiExtension.Result;
                //    var error = problemDetails.AdditionalProperties.ContainsKey("error") ? problemDetails.AdditionalProperties["error"].ToString() : problemDetails.AdditionalProperties["errors"].ToString();
                //    ToastService.ShowError(error);
                //}

            }
            else if (DataType == DataLibraryEnum.EmployeeRecords.ToString())
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
                        Category = "Employee Records",
                        Name = model.Value,
                        Section = model.Section,
                        Office = model.Office
                    };
                    await ExceptionHandlerService.HandleApiException(async () =>
                    {

                        var res = await RecordTypesService.CreateRecordTypesAsync(data);
                    }, null, $"{data.Name} Successfully Saved!");
                }
            }


        }
        IsLoading = false;
        await LoadLibraryData();
    }

    protected async Task SectionDropdownErrorChecker()
    {
        IsSectionEmpty = string.IsNullOrEmpty(NewConfig.Section);

        dialogReference.Refresh();
    }
    protected async Task OfficeDropdownErrorChecker()
    {
        IsOfficeEmpty = string.IsNullOrEmpty(NewConfig.Office);

        dialogReference.Refresh();
    }
}
