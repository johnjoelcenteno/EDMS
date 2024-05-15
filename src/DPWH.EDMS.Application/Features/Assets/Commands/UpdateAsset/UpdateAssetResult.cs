using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Assets.Commands.UpdateAsset;

public record UpdateAssetResult
{
    public UpdateAssetResult(Asset entity)
    {
        Id = entity.Id;
    }

    public Guid Id { get; set; }
}