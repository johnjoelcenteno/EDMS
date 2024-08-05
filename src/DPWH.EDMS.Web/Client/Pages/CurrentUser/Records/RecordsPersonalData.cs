using AutoMapper;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordManagement;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records
{
    public class RecordsPersonalData : GridBase<LookupRecordModels>
    {
        [Parameter]
        public string Id { get; set; }
        [Inject] public required ILookupsService LookupsService { get; set; }
        [Inject] public required IRecordManagementService RecordManagementService { get; set; }
        [Inject] public required IMapper Mapper { get; set; }
        protected LookupRecordModels EmployeeRecord { get; set; } = new LookupRecordModels();
        protected PersonalRecordDocument Record { get; set; } = new PersonalRecordDocument();
        protected Document Document { get; set; }
        protected DataSourceRequest body { get; set; } = new DataSourceRequest();
        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;
            //Record = MockCurrentData.GenerateCurrentDocuments().FirstOrDefault(x => x.Id == Id);
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
                Text = EmployeeRecord.RecordName,
                Url = "/record"
            });

            IsLoading = false;
        }


        protected async Task GetEmployeeRecords()
        {
            ServiceCb = RecordManagementService.Query;
            var filters = new List<Api.Contracts.Filter>();
            AddTextSearchFilterIfNotNull(filters, nameof(LookupRecordModels.Id), Id, "eq");
            SearchFilterRequest.Logic = DataSourceHelper.AND_LOGIC;
            SearchFilterRequest.Filters = filters.Any() ? filters : null;
            await LoadData();
            var docId = Guid.Parse(Id);
            //EmployeeRecord = res.Data?.FirstOrDefault(x => x.Id == docId) ?? null;
            EmployeeRecord = GridData?.FirstOrDefault(x => x.Id == docId) ?? null;

        }
        private void AddTextSearchFilterIfNotNull(List<Api.Contracts.Filter> filters, string fieldName, string? value, string operation)
        {
            if (!string.IsNullOrEmpty(value))
            {
                AddTextSearchFilter(filters, fieldName, value, operation);
            }
        }
    }
}
