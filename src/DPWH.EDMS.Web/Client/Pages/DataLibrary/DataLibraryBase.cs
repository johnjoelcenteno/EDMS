using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components;
using Microsoft.AspNetCore.Components;
using Telerik.FontIcons;

namespace DPWH.EDMS.Web.Client.Pages.DataLibrary;

public class DataLibraryBase : RxBaseComponent
{
    [Inject] protected IConfiguration Configuration { get; set; } = default!;

    protected List<DataLibraryModel> DataLibraryList = new List<DataLibraryModel>();

    protected override void OnInitialized()
    {
        DataLibraryList = new List<DataLibraryModel>
        {
            new DataLibraryModel
            {
                Icon = FontIcon.Gear,
                Name = "Record Types",
                Url = "/data-library/record-types"
            },
            new DataLibraryModel
            {
                Icon = FontIcon.Gear,
                Name = "Valid IDs",
                Url = "/data-library/valid-ids"
            },
            new DataLibraryModel
            {
                Icon = FontIcon.Gear,
                Name = "Authorization Documents",
                Url = "/data-library/authorization-documents"
            },
            new DataLibraryModel
            {
                Icon = FontIcon.Gear,
                Name = "Purposes",
                Url = "/data-library/purposes"

            }
        };

        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Data Library",
            Url = "/data-library"
        });
    }
}
