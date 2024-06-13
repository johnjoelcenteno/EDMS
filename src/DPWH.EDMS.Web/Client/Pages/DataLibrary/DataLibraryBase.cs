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
                Icon = FontIcon.FileConfig,
                Name = "Document Type",
                Url = "/data-library/document-type"
            },
            new DataLibraryModel
            {
                Icon = FontIcon.Gear,
                Name = "Purpose",
                Url = "/data-library/purpose"
            },
        };

        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "group",
            Text = "Data Library",
            Url = "/data-library"
        });
    }
}
