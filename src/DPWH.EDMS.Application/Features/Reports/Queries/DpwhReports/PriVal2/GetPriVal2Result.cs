using DPWH.EDMS.Application.Features.Assets.Queries;

namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriVal2;

public class GetPriVal2Result : PriReport
{
    public List<AssetImageModel> Images { get; set; } = new();
}