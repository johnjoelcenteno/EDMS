using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Components.Components.ReusableGrid;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.ViewRecords;

public class ViewRecordsBase : GridBase<RecordModel>
{
    [Parameter]
    public string Id { get; set; }
    [Parameter]
    public int DocumentId { get; set; }
    [Inject] public required IUsersService UsersService { get; set; }
    protected RecordModel Record { get; set; } = new RecordModel();
    protected Document Document { get; set; } = new Document();
    protected List<RecordModel> RecordsList = new List<RecordModel>();
     
    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        await GetEmployeeDetails();
        if (Record != null)
        {
            Document = MockData.GenerateDocuments().FirstOrDefault(x => x.Id == DocumentId);
            if (Document == null)
            {
                Console.WriteLine($"No document found with Id: {DocumentId}");
            }

        }
        IsLoading = false;
    }
    protected override void OnParametersSet()
    {
        //RecordsList = MockData.GetRecords().ToList();
        //Record = RecordsList.FirstOrDefault(r => r.Id == Id);
        
         
        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Records Management",
            Url = "/records-management"
        });

        BreadcrumbItems.Add(new BreadcrumbModel
        {
            Icon = "menu",
            Text = "Records",
            Url = $"records-management/{Id}"
        });
         
    }
    private async Task GetEmployeeDetails()
    {
       var res = await UsersService.GetUserByEmployeeId(Id);
        if (res != null && res.Success)
        {
            if (res.Data != null)
            {
                Record.FirstName = res.Data.FirstName;
                Record.LastName = res.Data.LastName;
                Record.Role = res.Data.UserAccess;
            }
        }
        StateHasChanged();
    }
}
