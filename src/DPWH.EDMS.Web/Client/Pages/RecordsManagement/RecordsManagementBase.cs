using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using Telerik.Blazor.Components;
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
            Text = "Records Management",
            Url = "/records-management"
        });

        RecordList = GetRecords();
    }

    public List<FilterOperator> filterOperators { get; set; } = new List<FilterOperator>()
    {
        FilterOperator.IsEqualTo,
        FilterOperator.IsNotEqualTo,
        FilterOperator.StartsWith,
        FilterOperator.Contains,
        FilterOperator.DoesNotContain
    };

    public List<RecordModel> GetRecords()
    {
        return new List<RecordModel>
        {
            new RecordModel
            {
                Id = Guid.NewGuid(),
                LastName = "Rosales",
                FirstName = "Karen",
                MiddleName = "Mesinas",
                Office = "Headquarters",
                BureauServiceDivisionSectionUnit = "Administration Division"
            },
            new RecordModel
            {
                Id = Guid.NewGuid(),
                LastName = "Jeresano",
                FirstName = "Edmar",
                MiddleName = "Alano",
                Office = "Regional Office",
                BureauServiceDivisionSectionUnit = "Human Resources Bureau"
            },
            new RecordModel
            {
                Id = Guid.NewGuid(),
                LastName = "Aluan",
                FirstName = "Nataniel",
                MiddleName = "Girado",
                Office = "Branch Office",
                BureauServiceDivisionSectionUnit = "Finance Service"
            },
            new RecordModel
            {
                Id = Guid.NewGuid(),
                LastName = "Millano",
                FirstName = "Darwin",
                MiddleName = "Evangelista",
                Office = "District Office",
                BureauServiceDivisionSectionUnit = "Operations Division"
            },
            new RecordModel
            {
                Id = Guid.NewGuid(),
                LastName = "Belmonte",
                FirstName = "Kevin",
                MiddleName = "Santos",
                Office = "Satellite Office",
                BureauServiceDivisionSectionUnit = "Technical Support Unit"
            }
        };
    }

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
