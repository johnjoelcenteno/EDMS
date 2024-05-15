namespace DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriBomEval;

public class GetPriBomEvalResult : PriReport
{
    public decimal RequestedAmount { get; set; }
    public decimal EvaluatedAllocation { get; set; }
    public string Inventory { get; set; }
    public string ProofOfOwnership { get; set; }
    public string POW { get; set; }
    public string DUPA { get; set; }
    public string DetailedEstimates { get; set; }
    public string Picture { get; set; }
    public string Plan { get; set; }
    public string YearFunded { get; set; }
    public string CostOfRepair { get; set; }
    public string Remarks { get; set; }
}

public class GetPriBomEvalResultTotal
{
    public GetPriBomEvalResultTotal(List<GetPriBomEvalResult> priBomEval)
    {
        PriBomEval = priBomEval;
        TotalRequestAmount = ModelHelper.CalculateTotal(priBomEval.Select(p => p.RequestedAmount).ToList());
        TotalEvaluatedAllocation = ModelHelper.CalculateTotal(priBomEval.Select(p => p.EvaluatedAllocation).ToList());
    }

    public List<GetPriBomEvalResult> PriBomEval { get; set; }
    public decimal TotalRequestAmount { get; set; }
    public decimal TotalEvaluatedAllocation { get; set; }
}
