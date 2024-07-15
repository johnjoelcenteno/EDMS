using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records
{
    public class RecordsPersonalData : GridBase<PersonalRecordDocument>
    {
        [Parameter]
        public string Id { get; set; }
        [Inject] public required ILookupsService LookupsService { get; set; }
        protected RecordModel CurrentRecord = new RecordModel();
        protected Api.Contracts.GetLookupResult EmployeeRecord { get; set; } = new Api.Contracts.GetLookupResult();
        protected PersonalRecordDocument Record { get; set; } = new PersonalRecordDocument();
        protected Document Document { get; set; }
        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;
            Record = MockCurrentData.GenerateCurrentDocuments().FirstOrDefault(x => x.Id == Id);
            //await GetEmployeeRecords();

            BreadcrumbItems.Add(new BreadcrumbModel
            {
                Icon = "menu",
                Text = "My Records",
                Url = "/my-records"
            });
            BreadcrumbItems.Add(new BreadcrumbModel
            {
                Icon = "menu",
                Text = Record.DocumentName,
                Url = "/record"
            });

            IsLoading = false;
        }
      

        //protected async Task GetEmployeeRecords()
        //{
        //    var res = await LookupsService.GetEmployeeRecords();
        //    if (res.Success && res.Data != null)
        //    {
        //        var docId = Guid.Parse(Id);
        //        EmployeeRecord = res.Data?.FirstOrDefault(x => x.Id == docId) ?? null;

        //    }
        //}
    }
}
