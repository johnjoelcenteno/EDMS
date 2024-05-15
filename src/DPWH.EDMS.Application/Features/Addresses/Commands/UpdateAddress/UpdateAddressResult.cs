using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Addresses.Commands.UpdateAddress;

public record UpdateAddressResult
{
    public UpdateAddressResult(GeoLocation entity)
    {
        Id = entity.MyId;
        Name = entity.Name;
        Type = entity.Type;
        ParentId = entity.ParentId;
    }

    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? ParentId { get; set; }
}