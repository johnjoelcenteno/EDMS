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

namespace DPWH.EDMS.Web.Client.Pages.CurrentUser.Records;

public class RecordsBase : GridBase<EmployeeSamp>
{
    [CascadingParameter] private Task<AuthenticationState>? AuthenticationStateAsync { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }
    
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
        EmployeeList = GenerateEmployeeRecords(5);
    }

    public async Task exportData(GridCommandEventArgs args)
    {
        EmployeeSamp selectedId = args.Item as EmployeeSamp;
        
        //Int32.TryParse(samp, out sampNumber);
        NavigationManager.NavigateTo($"/personal-Data/{selectedId.EmployeeId}");
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
