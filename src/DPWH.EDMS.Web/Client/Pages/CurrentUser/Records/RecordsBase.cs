using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using static DPWH.EDMS.Web.Client.Pages.CurrentUser.Records.RecordsBase;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records;

public class RecordsBase : GridBase<Document>
{
    [Parameter] public required string Id { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    protected List<Document> DocumentList = new List<Document>();

    public class Document
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string ControlNumber { get; set; }
        public DateTime DateRequested { get; set; }
    }

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Records",
            Url = "/my-records"
        });

        DocumentList = GetDocuments();
    }

    protected List<Document> GetDocuments()
    {
        return new List<Document>
        {
            new Document { Id = 1, DocumentName = "Approved Appointments", ControlNumber = "CN001", DateRequested = DateTime.Now },
            new Document { Id = 2, DocumentName = "Position Description Form (PDF)", ControlNumber = "CN002", DateRequested = DateTime.Now },
            new Document { Id = 2, DocumentName = "Service Records", ControlNumber = "CN003", DateRequested = DateTime.Now },
            new Document { Id = 4, DocumentName = "Employee Leave Card", ControlNumber = "CN004", DateRequested = DateTime.Now },
            new Document { Id = 5, DocumentName = "Leave Application (Terminal Leave only)", ControlNumber = "CN005", DateRequested = DateTime.Now },
            new Document { Id = 6, DocumentName = "Notice of Salary Adjustment (NOSA)", ControlNumber = "CN006", DateRequested = DateTime.Now },
            new Document { Id = 7, DocumentName = "Personal Data Sheet (PDS) / Information Sheet (must be latest)", ControlNumber = "CN007", DateRequested = DateTime.Now },
            new Document { Id = 8, DocumentName = "Administrative Case / Civil Case/Decisions", ControlNumber = "CN008", DateRequested = DateTime.Now },
            new Document { Id = 9, DocumentName = "Training Certificates / Ratings", ControlNumber = "CN009", DateRequested = DateTime.Now },
            new Document { Id = 10, DocumentName = "Travel Directive, Certificate of Appearance", ControlNumber = "CN010", DateRequested = DateTime.Now },
            new Document { Id = 11, DocumentName = "Authority to Travel for Personal Reason", ControlNumber = "CN011", DateRequested = DateTime.Now },
            new Document { Id = 12, DocumentName = "Memorandum Receipt for Equipment (MR)", ControlNumber = "CN012", DateRequested = DateTime.Now },
            new Document { Id = 13, DocumentName = "GSIS Forms (Retirement, Information for Membership)", ControlNumber = "CN013", DateRequested = DateTime.Now },
            new Document { Id = 14, DocumentName = "Memo (Designation, Directive)", ControlNumber = "CN014", DateRequested = DateTime.Now },
            new Document { Id = 15, DocumentName = "Daily Wage Appointment/Plantilla", ControlNumber = "CN015", DateRequested = DateTime.Now }
        };
    }

    public async Task viewData(GridCommandEventArgs args)
    {
        Document selectedId = args.Item as Document;

        //Int32.TryParse(samp, out sampNumber);
        NavigationManager.NavigateTo($"/personal-data/{selectedId.Id}");
    }
}
