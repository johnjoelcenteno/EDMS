using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Components;
using Microsoft.AspNetCore.Components;

namespace DPWH.EDMS.Web.Client.Pages.RecordsManagement.Employee.PersonalDetails;

public class PersonalDetailsBase: RxBaseComponent
{
    [Parameter] public required GetUserByIdResult SelectedEmployee { get; set; }
}
