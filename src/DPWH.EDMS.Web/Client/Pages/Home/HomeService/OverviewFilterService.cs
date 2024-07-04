using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;

namespace DPWH.EDMS.Web.Client.Pages.Home.HomeService;
public class OverviewFilterService
{
    public Filter GetFilterOverviewBanner()
    {
        var filter = new Filter
        {
            Logic = "or",
            Filters = new List<Filter>
                {

                    new Filter
                    {
                        Field = nameof(EmployeeModel.Status),
                        Operator = "eq",
                        Value = "Review"
                    },
                    new Filter
                    {
                        Field = nameof(EmployeeModel.Status),
                        Operator = "eq",
                        Value = "Release"
                    },
                    new Filter
                    {
                        Field = nameof(EmployeeModel.Status),
                        Operator = "eq",
                        Value = "Claimed"
                    }
                }
        };

        return filter;
    }
    
    //Adding More Service In Future
}