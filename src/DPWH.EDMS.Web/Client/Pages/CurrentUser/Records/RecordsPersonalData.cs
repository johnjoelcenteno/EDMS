using DPWH.EDMS.Components;
using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Telerik.SvgIcons;
using DPWH.EDMS.IDP.Core.Constants;
using Telerik.Blazor.Components;
using Telerik.DataSource;
using DPWH.EDMS.Client.Shared.Models;
using Microsoft.AspNetCore.Http.Extensions;
using static Telerik.Blazor.ThemeConstants;
using static DPWH.EDMS.Web.Client.Pages.CurrentUser.Records.RecordsBase;
using Telerik.Blazor.Components.Breadcrumb;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records
{
    public class RecordsPersonalData : GridBase<EmployeeSamp>
    {
        [Parameter]
        public int Id { get; set; }
        public class EmployeeSamp
        {
            public int EmployeeId { get; set; }
            public string ControlNumber { get; set; }
            public DateTime DateRequested { get; set; }
            public string DocumentName { get; set; }
        }
        protected List<EmployeeSamp> EmployeeList = new List<EmployeeSamp>();
        protected override void OnInitialized()
        {
            BreadcrumbItems.Add(new BreadcrumbModel
            {
                Icon = "menu",
                Text = "My Records",
                Url = "/my-Records"
            });
            
            BreadcrumbItems.Add(new BreadcrumbModel
            {
                Icon = "folder",
                Text = "Personal Data",
                Url = "/personal-Data"
            });
            EmployeeList = GenerateEmployeeRecords(5);
        }
        private List<EmployeeSamp> GenerateEmployeeRecords(int count)
        {
            var employees = new List<EmployeeSamp>();
            var random = new Random();

            for (int i = 1; i <= count; i++)
            {
                var employee = new EmployeeSamp
                {
                    EmployeeId = i,
                    ControlNumber = $"CN{i:D4}",
                    DateRequested = DateTime.Now.AddDays(-random.Next(0, 365)),
                    DocumentName = $"DocumentName{i}"
                };

                employees.Add(employee);
            }

            return employees;
        }
    }
}
