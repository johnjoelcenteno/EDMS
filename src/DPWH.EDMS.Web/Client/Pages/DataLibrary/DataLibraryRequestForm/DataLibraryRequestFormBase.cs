using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.Common.Enum;
using DPWH.EDMS.Web.Client.Shared.DataLibrary.RequestForm;
using System.Text.RegularExpressions;

namespace DPWH.EDMS.Web.Client.Pages.DataLibrary.DataLibraryRequestForm;

public class DataLibraryRequestFormBase : DataLibraryRequestFormComponentBase
{
    protected override async void OnInitialized()
    {
        IsLoading = true;
        await LoadSection();
        await LoadOffice();
        await GetCurrentValues();
        IsVisible = true;
        IsLoading = false;
    }


    #region Load Current Value
    protected async Task GetCurrentValues()
    {
       if(Type == "Edit")
       {
            NewConfig.Value = EditItem.Value;
            NewConfig.Id = EditItem.Id.ToString();
            if (DataType == DataLibraryEnum.EmployeeRecords.ToString())
            {
                    NewConfig.Section = EditItem.Section;
                    NewConfig.Office = EditItem.Office;
            }
        }
        NewConfig.DataType = DataType;
    }
    #endregion

}

