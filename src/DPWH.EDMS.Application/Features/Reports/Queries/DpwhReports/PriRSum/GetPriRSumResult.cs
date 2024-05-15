namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriRSum;

public class GetPriRSumResult : PriReport
{
    public int NoOfBuildings { get; set; }
    public decimal EstimatedCostImplementingOffice { get; set; }
    public decimal EstimatedCostRegionalOffice { get; set; }
    public string? POW { get; set; }
    public string? DUPA { get; set; }
    public string? DetailedEstimates { get; set; }
    public string? Plans { get; set; }
    public string? NBSDNGOBPRIForm { get; set; }
    public string? Remarks { get; set; }
}

public class GetPriRSumResultTotal
{
    public GetPriRSumResultTotal(List<GetPriRSumResult> priRSum)
    {
        PriRSum = priRSum;
        TotalBuildings = ModelHelper.CalculateTotal(priRSum.Select(p => p.NoOfBuildings).ToList());
        TotalEstimatedCostImplementingOffice = ModelHelper.CalculateTotal(priRSum.Select(p => p.EstimatedCostImplementingOffice).ToList());
        TotalEstimatedCostRegionalOffice = ModelHelper.CalculateTotal(priRSum.Select(p => p.EstimatedCostRegionalOffice).ToList());
    }

    public List<GetPriRSumResult> PriRSum { get; set; }
    public decimal TotalEstimatedCostImplementingOffice { get; set; }
    public decimal TotalEstimatedCostRegionalOffice { get; set; }
    public int TotalBuildings { get; set; }
}
