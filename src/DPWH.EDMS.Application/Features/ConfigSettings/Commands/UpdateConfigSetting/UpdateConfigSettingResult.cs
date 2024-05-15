using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Commands.UpdateConfigSetting;

public record UpdateConfigSettingResult
{
    public UpdateConfigSettingResult(ConfigSetting entity)
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