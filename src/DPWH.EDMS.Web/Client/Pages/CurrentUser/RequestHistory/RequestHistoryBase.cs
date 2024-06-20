using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Telerik.DataSource;

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.RequestHistory;

public class RequestHistoryBase: GridBase<EmployeeModel>
{
    protected List<EmployeeModel> RecordList = new List<EmployeeModel>();
    public string DropDownListValue { get; set; }

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "My Request History",
            Url = "/my-request-history"
        });

        RecordList = GenerateRecords(5);
    }

    public List<string> RecordTypes = new List<string>
    {
        "Appointment Papers", "Leave Card", "Memorandum", "PDS",
        "Salary Adjustment", "Service Record", "Training Certificates"
    };

    protected List<EmployeeModel> GenerateRecords(int count)
    {

        var records = new List<EmployeeModel>();
        var random = new Random();

        for (int i = 1; i <= count; i++)
        {
            var record = new EmployeeModel
            {
                ControlNumber = $"CN000{i}",
                RecordRequested = RecordTypes[random.Next(RecordTypes.Count)],
                DateRequested = DateTime.Now.AddDays(-random.Next(0, 365)),
                Purpose = $"Purpose{random.Next(1, 5)}",
                Status = random.Next(0, 2) == 0 ? "Pending" : "Completed",
            };

            records.Add(record);
        }

        return records;
    }
}
