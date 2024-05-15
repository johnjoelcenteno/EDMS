using DPWH.EDMS.Application.Models;

namespace DPWH.EDMS.Application.Features.Reports.Queries;

public class ReportResponse : BaseResponse
{
    public AssestPerConditionModel Model { get; }

    public ReportResponse(AssestPerConditionModel model)
    {
        Model = model;
    }
}

