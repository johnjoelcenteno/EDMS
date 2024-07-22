using DPWH.EDMS.Web.Client.Shared.RecordRequest.RequestForm;

namespace DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.Common.Components.RequestForm;

public class RecordTypesRequestFormBase : RecordTypesFormComponentBase
{

    protected override async Task OnInitializedAsync()
    {
        await LoadSection();
        await LoadOffice();
        await LoadCurrentValues();
        DialogReference.Refresh();
        StateHasChanged();
    }
}

