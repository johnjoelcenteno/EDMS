using DPWH.EDMS.Application.Features.Assets.Queries;

namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriInd;

public class GetPriIndResult : PriReport
{
    public List<AssetImageModel> Images { get; set; } = new();
}
