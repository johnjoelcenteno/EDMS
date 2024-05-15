using DPWH.EDMS.Application.Features.Assets.Queries;
using DPWH.EDMS.Application.Models;

namespace DPWH.EDMS.Application.Features.Assets.Models;

public class AssetResponse : BaseResponse
{
    public AssetModel Model { get; }

    public AssetResponse(AssetModel model)
    {
        Model = model;
    }
}