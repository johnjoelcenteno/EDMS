using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records
{
    public class RecordsPersonalData : GridBase<Document>
    {
        [Parameter]
        public int Id { get; set; }
        protected RecordModel CurrentRecord = new RecordModel();
        protected Document Document { get; set; }
        protected override void OnInitialized()
        {
            
        }

        protected override void OnParametersSet()
        {
            Document = MockCurrentData.GetDocuments().FirstOrDefault(d => d.Id == Id);

            BreadcrumbItems.Add(new BreadcrumbModel
            {
                Icon = "menu",
                Text = "My Records",
                Url = "/my-records"
            });

            BreadcrumbItems.Add(new BreadcrumbModel
            {
                Icon = "menu",
                Text = Document.DocumentName,
                Url = "/record"
            });
        }
    }
}
