using DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;

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
}
