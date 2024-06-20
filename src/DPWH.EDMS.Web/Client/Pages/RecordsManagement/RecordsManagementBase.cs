using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Telerik.DataSource;


namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement;

public class RecordsManagementBase : GridBase<RecordModel>
{
    protected List<RecordModel> RecordList = new List<RecordModel>();
    public FilterOperator filterOperator { get; set; } = FilterOperator.StartsWith;


    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Record Management",
            Url = "/records-management"
        });

        RecordList = GenerateRecords(5);
    }

    public List<FilterOperator> filterOperators { get; set; } = new List<FilterOperator>()
    {
        FilterOperator.IsEqualTo,
        FilterOperator.IsNotEqualTo,
        FilterOperator.StartsWith,
        FilterOperator.Contains,
        FilterOperator.DoesNotContain
    };

    protected List<RecordModel> GenerateRecords(int count)
    {
        var records = new List<RecordModel>();
        var random = new Random();

        for (int i = 1; i <= count; i++)
        {
            var record = new RecordModel
            {
                LastName = $"LastName{i}",
                FirstName = $"FirstName{i}",
                MiddleName = $"MiddleName{i}",
                Office = $"Office{random.Next(1, 5)}",
                BureauServiceDivisionSectionUnit = $"BureauServiceDivisionSectionUnit{random.Next(1, 5)}"
            };

            records.Add(record);
        }

        return records;
    }
}
