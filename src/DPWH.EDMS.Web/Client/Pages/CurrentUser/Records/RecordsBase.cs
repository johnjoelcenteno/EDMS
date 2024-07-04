using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records;

public class RecordsBase : GridBase<Document>
{
    [Parameter] public required string Id { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    protected EDMS.Client.Shared.MockModels.RecordModel Record { get; set; }
    protected MockCurrentData CurrentData { get; set; }
    protected List<Document> DocumentList = new List<Document>();

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Records",
            Url = "/my-records"
        });

        Record = MockCurrentData.GetCurrentRecord();
        DocumentList = MockCurrentData.GetDocuments().OrderBy(d => d.DocumentName).ToList();
    }

    public async Task viewData(GridCommandEventArgs args)
    {
        Document selectedId = args.Item as Document;

        //Int32.TryParse(samp, out sampNumber);
        NavigationManager.NavigateTo($"/my-records/{selectedId.Id}");
    }
}
