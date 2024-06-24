using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.EDMS.Components.Helpers;
using Microsoft.AspNetCore.Components;
using Telerik.SvgIcons;

namespace DPWH.EDMS.Web.Client.Pages.RequestManagement.ReviewRequestForm
{
    public class ReviewRequestFormBase : GridBase<DocumentRequestModel>
    {
        [Parameter] public required string ControlNumber { get; set; }
        protected List<EmployeeModel> RequestList = new List<EmployeeModel>();
        protected EmployeeModel SelectedRecordRequest { get; set; } = new();
        protected override void OnInitialized()
        {
            BreadcrumbItems.AddRange(new List<BreadcrumbModel>
            {
                new BreadcrumbModel
                {
                    Icon = "menu",
                    Text = "Request Management",
                    Url = "/Request-management"
                },
                new BreadcrumbModel
                {
                    Icon = "create_new_folder",
                    Text = "Request Review Form",
                    Url = "/review-request",
                }
            });

            RequestList = GenerateEmployeeRecords(5);
            SelectedRecordRequest = RequestList.Single(c => c.ControlNumber == ControlNumber);

        }
        private List<EmployeeModel> GenerateEmployeeRecords(int count)
        {
            var employees = new List<EmployeeModel>();
            var random = new Random();

            for (int i = 1; i <= count; i++)
            {
                var employee = new EmployeeModel
                {
                    ControlNumber = $"CN{i:D4}",
                    DateRequested = DateTime.Now.AddDays(-random.Next(0, 365)),
                    LastName = $"Office{i}",
                    FullName = $"FirstName{i} M. LastName{i}",
                    MiddleInitial = random.Next(0, 2) == 0 ? "Yes" : "No",
                    RecordRequested = $"RecordType{random.Next(1, 5)}",
                    Purpose = $"Purpose{random.Next(1, 5)}",
                    Status = random.Next(0, 2) == 0 ? "Pending" : "Completed",
                    UserAccess = "EndUser"
                };

                employees.Add(employee);
            }

            return employees;
        }
    }
}
