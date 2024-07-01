using DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.RequestForm;

public class RequestFormBase : RequestFormComponentBase
{
   protected override void OnInitialized()
    {
        LoadClaimantTypes();
        _SetDefaultRequestDate();
    }

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await LoadValidIDTypes();
        await LoadAuthorizeDocumentTypes();
        await LoadRecordTypes();        
        IsLoading = false;
    }

    #region Load Events
    private void _SetDefaultRequestDate()
    {
        if (SelectedItem.DateRequested == default)
        {
            SelectedItem.DateRequested = DateTime.Now;
        }
    }
    #endregion
}
