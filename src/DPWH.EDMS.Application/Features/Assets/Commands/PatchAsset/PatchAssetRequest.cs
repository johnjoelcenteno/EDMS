using System.Text.Json;

namespace DPWH.EDMS.Application.Features.Assets.Commands.PatchAsset;

public record PatchAssetRequest
{
    public List<PatchOperation> Operations { get; set; } = new();
}

public record PatchOperation
{
    public string PropertyName { get; set; }
    public JsonElement Value { get; set; }
}