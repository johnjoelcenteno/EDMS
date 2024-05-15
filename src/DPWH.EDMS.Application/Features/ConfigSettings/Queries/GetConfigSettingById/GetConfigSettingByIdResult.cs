using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Queries.GetConfigSettingById;

public record GetConfigSettingByIdResult
{
    public GetConfigSettingByIdResult(ConfigSetting entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Value = entity.Value;
        Description = entity.Description;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
}