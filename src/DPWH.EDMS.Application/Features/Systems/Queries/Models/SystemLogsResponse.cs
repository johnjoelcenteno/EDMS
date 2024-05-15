using DPWH.EDMS.Application.Models;

namespace DPWH.EDMS.Application.Features.Systems.Queries.Models;

public class SystemLogsResponse : BaseResponse
{
    public SystemLogsModel Model { get; }

    public SystemLogsResponse(SystemLogsModel model)
    {
        Model = model;
    }
}

