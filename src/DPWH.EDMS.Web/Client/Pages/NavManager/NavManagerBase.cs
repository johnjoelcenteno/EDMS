using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.NavManager;

public class NavManagerBase : GridBase<MenuItemModel>
{
    [Inject] public required INavigationService NavigationService {  get; set; }
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
}
