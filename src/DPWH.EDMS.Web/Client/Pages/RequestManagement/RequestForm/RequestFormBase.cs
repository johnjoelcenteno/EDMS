using DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.RequestForm;

public class RequestFormBase : RequestFormComponentBase
{
   protected override void OnInitialized()
    {
        HandleOnInit();
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await HandleLoadItems();     
        IsLoading = false;
    }
}
