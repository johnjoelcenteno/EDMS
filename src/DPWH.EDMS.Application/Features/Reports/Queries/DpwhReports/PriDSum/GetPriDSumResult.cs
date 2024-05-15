namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriDSum;

public class GetPriDSumResult : PriReport
{
    public int NoOfBuildings { get; set; }
    public decimal EstimatedCost { get; set; }
    public string? ScopeOfWork { get; set; }
}

public class GetPriDSumResultTotal
{
    public GetPriDSumResultTotal(List<GetPriDSumResult> priDSum)
    {
        PriDSum = priDSum;
        TotalBuildings = ModelHelper.CalculateTotal(priDSum.Select(p => p.NoOfBuildings).ToList());
        TotalCost = ModelHelper.CalculateTotal(priDSum.Select(p => p.EstimatedCost).ToList());
    }

    public List<GetPriDSumResult> PriDSum { get; set; }
    public int TotalBuildings { get; set; }
    public decimal TotalCost { get; set; }
}
