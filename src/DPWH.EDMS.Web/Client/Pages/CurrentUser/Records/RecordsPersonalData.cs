using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records
{
    public class RecordsPersonalData : GridBase<Document>
    {
        [Parameter]
        public string Id { get; set; }
        [Inject] public required ILookupsService LookupsService { get; set; }
        protected RecordModel CurrentRecord = new RecordModel();
        protected Api.Contracts.GetLookupResult EmployeeRecord { get; set; } = new Api.Contracts.GetLookupResult();
        protected Document Document { get; set; }
        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;

            await GetEmployeeRecords();

            BreadcrumbItems.Add(new BreadcrumbModel
            {
                Icon = "menu",
                Text = "My Records",
                Url = "/my-records"
            });
            BreadcrumbItems.Add(new BreadcrumbModel
            {
                Icon = "menu",
                Text = EmployeeRecord.Name,
                Url = "/record"
            });

            IsLoading = false;
        }
      

        protected async Task GetEmployeeRecords()
        {
            var res = await LookupsService.GetEmployeeRecords();
            if (res.Success && res.Data != null)
            {
                var docId = Guid.Parse(Id);
                EmployeeRecord = res.Data?.FirstOrDefault(x => x.Id == docId) ?? null;

            }
        }
    }
}
