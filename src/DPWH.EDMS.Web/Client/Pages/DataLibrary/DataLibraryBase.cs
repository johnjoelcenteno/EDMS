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
                Icon = FontIcon.Dimensions,
                Name = "Data 1",
                Url = "/"
            },
            new DataLibraryModel
            {
                Icon = FontIcon.Sliders,
                Name = "Data 2",
                Url = "/"
            },
            new DataLibraryModel
            {
                Icon = FontIcon.Calendar,
                Name = "Data 3",
                Url = "/"
            },
            new DataLibraryModel
            {
                Icon = FontIcon.Gear,
                Name = "Data 4",
                Url = "/"
            }
        };

        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "group",
            Text = "Data Library",
            Url = "/data-library"
        });
    }
}
