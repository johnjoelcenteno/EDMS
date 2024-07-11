namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetMonthlyRequests;

public record GetMonthlyRequestModel
{
    public string Month {  get; set; }
    public int Count { get; set; }
}
