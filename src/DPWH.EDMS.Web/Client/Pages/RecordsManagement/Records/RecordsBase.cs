using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.Records;

public class RecordsBase : GridBase<Document>
{
    [Parameter] public required string Id { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    protected List<Document> DocumentList = new List<Document>();
    protected RecordModel Record { get; set; } = new RecordModel();

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Records Management",
            Url = "/records-management"
        });

        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Records",
            Url = "/records"
        });
    }

    protected override void OnParametersSet()
    {
        var records = MockData.GetRecords();
        Record = records.FirstOrDefault(r => r.Id == Id);
        DocumentList = Record?.Documents ?? new List<Document>();
    }

    public async Task viewData(GridCommandEventArgs args)
    {
        Document selectedId = args.Item as Document;

        //Int32.TryParse(samp, out sampNumber);
        NavigationManager.NavigateTo($"/records-management/{Id}/{selectedId.Id}");
    }
}