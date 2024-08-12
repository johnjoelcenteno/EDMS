using AutoMapper;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Menu;
using Telerik.SvgIcons;

namespace DPWH.EDMS.Web.Client.Pages.NavManager;

public class NavManagerBase : GridBase<MenuItemModel>
{
    [Inject] public required INavigationService NavigationService {  get; set; }
    [Inject] public required IJSRuntime JSRuntime {  get; set; }
    [Inject] public required IDocumentService DocumentService { get; set; }
    [Inject] public required IMapper Mapper { get; set; }

    protected List<string> AllowedExtensions { get; set; } = new List<string>() { ".json" };

    protected async override Task OnInitializedAsync()
    {
        IsLoading = true;
        ServiceCb = NavigationService.Query;
        await LoadData();
        StateHasChanged();
        IsLoading = false;
    }

    protected void GoToCreateMenuItem()
    {
        NavManager.NavigateTo("/navmanager/create");
    }

    protected void GoToEditMenuItem(Guid id)
    {
        NavManager.NavigateTo($"/navmanager/edit/{id.ToString()}");
    }

    protected void HandleEditMenuItem(GridCommandEventArgs args)
    {
        var item = (MenuItemModel)args.Item;
        GoToEditMenuItem(item.Id);
    }

    protected async Task HandleDeleteMenuItem(GridCommandEventArgs args)
    {
        IsLoading = true;
        var item = (MenuItemModel)args.Item;
        var res = await NavigationService.Delete(item.Id);

        if(res.Success)
        {
            ToastService.ShowSuccess("Successfully deleted menu item!");
        }
        else
        {
            ToastService.ShowError("Something went wrong on deleting menu item!");
        }

        await LoadData();
        StateHasChanged();
        IsLoading = false;
    }

    protected async Task ExportMenuJson()
    {
        var res = (await NavigationService.Query(new DataSourceRequest() { Skip = 0 })).Data.ToList();
        var mappedRes = GenericHelper.GetListByDataSource<MenuItemModel>(res);

        string jsonString = JsonConvert.SerializeObject(mappedRes, Formatting.Indented);
        byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(jsonString);
        string fileName = "MenuItems.json";

        await JSRuntime.InvokeVoidAsync("saveAsFile", fileName, Convert.ToBase64String(fileBytes));
    }

    protected async void OnUpload(FileSelectEventArgs args)
    {
        var file = await DocumentService.GetFileToUpload(args);
        Stream data = file.Data;

        // Read the file's content
        using var reader = new StreamReader(data, Encoding.UTF8);
        var jsonString = await reader.ReadToEndAsync();

        // Deserialize the JSON content into a list of MenuItemModel
        List<MenuItemModel> res = JsonConvert.DeserializeObject<List<MenuItemModel>>(jsonString) ?? new List<MenuItemModel>();
        
        IsLoading = true;
        if ( res.Count() > 0) {
            foreach (var item in res)
            {
                var createRes = Mapper.Map<CreateMenuItemModel>(item);
                var req = await NavigationService.Create(createRes);

                if (!req.Success) { break; }
            }
        }
        IsLoading = false;
        //ToastService.ShowInfo("Imported menu items from selected json file.");
        NavManager.Refresh();
    }
}
