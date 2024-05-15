using DPWH.EDMS.Domain.Entities;
namespace DPWH.EDMS.Application.Features.Assets.Commands.ValidateAsset;

public record ValidateAssetResult
{
    public ValidateAssetResult(Asset entity)
    {
        Id = entity.Id;
    }

    public Guid Id { get; set; }
}