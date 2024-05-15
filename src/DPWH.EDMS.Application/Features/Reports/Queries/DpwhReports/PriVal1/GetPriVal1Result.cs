namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriVal1;

public class GetPriVal1Result : PriReport
{
    public int NoOfBuildings { get; set; }
    public bool IncludedPriorityList { get; set; }
    public bool Validated { get; set; }
    public bool FundingRecommended { get; set; }
    public decimal ProposedMaintenanceCost { get; set; }
    public decimal EvaluatedMaintenanceCost { get; set; }
    public string? Remarks { get; set; }
}

public class GetPriVal1ResultTotal
{
    public GetPriVal1ResultTotal(List<GetPriVal1Result> priVal1)
    {
        PriVal1 = priVal1;
        TotalBuildings = ModelHelper.CalculateTotal(priVal1.Select(p => p.NoOfBuildings).ToList());
        TotalProposedMaintenanceCost = ModelHelper.CalculateTotal(priVal1.Select(p => p.ProposedMaintenanceCost).ToList());
        TotalEvaluatedMaintenanceCost = ModelHelper.CalculateTotal(priVal1.Select(p => p.EvaluatedMaintenanceCost).ToList());
    }

    public List<GetPriVal1Result> PriVal1 { get; set; }
    public int TotalBuildings { get; set; }
    public decimal TotalProposedMaintenanceCost { get; set; }
    public decimal TotalEvaluatedMaintenanceCost { get; set; }
}