using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Telerik.Blazor.Components;
using Telerik.DataSource;


namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement;

public class RecordsManagementBase : GridBase<RecordModel>
{
    protected MockData MockData { get; set; }
    protected List<RecordModel> RecordList = new List<RecordModel>();
    public FilterOperator filterOperator { get; set; } = FilterOperator.StartsWith;


    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Records Management",
            Url = "/records-management"
        });

        MockData.GetRecords();
    }

    public List<FilterOperator> filterOperators { get; set; } = new List<FilterOperator>()
    {
        FilterOperator.IsEqualTo,
        FilterOperator.IsNotEqualTo,
        FilterOperator.StartsWith,
        FilterOperator.Contains,
        FilterOperator.DoesNotContain
    };

    protected void GoToSelectedItemDocuments(GridRowClickEventArgs args)
    {
        IsLoading = true;

        var selectedItem = args.Item as RecordModel;

        if (selectedItem != null)
        {
            NavManager.NavigateTo("records-management/" + selectedItem.Id.ToString());
        }

        IsLoading = false;
    }
}
