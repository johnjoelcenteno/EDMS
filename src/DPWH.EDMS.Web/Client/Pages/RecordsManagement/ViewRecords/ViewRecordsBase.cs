using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.ViewRecords;

public class ViewRecordsBase : GridBase<RecordModel>
{
    [Parameter]
    public string Id { get; set; }
    [Parameter]
    public int DocumentId { get; set; }
    protected RecordModel Record { get; set; }
    protected Document Document { get; set; }
    protected List<RecordModel> RecordsList = new List<RecordModel>();

    protected override void OnInitialized()
    {
        
    }

    protected override void OnParametersSet()
    {
        RecordsList = MockData.GetRecords().ToList();
        Record = RecordsList.FirstOrDefault(r => r.Id == Id);
        if (Record != null)
        {
            Document = Record.Documents.FirstOrDefault(d => d.Id == DocumentId);
            if (Document == null)
            {
                Console.WriteLine($"No document found with Id: {DocumentId}");
            }
        }
        else
        {
            Console.WriteLine($"No record found with Id: {Id}");
        }

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
            Url = $"records-management/{Id}"
        });

        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = Document.DocumentName,
            Url = "/records"
        });
    }
}
