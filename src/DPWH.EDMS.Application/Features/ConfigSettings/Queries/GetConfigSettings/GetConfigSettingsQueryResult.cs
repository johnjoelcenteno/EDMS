using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Queries.GetConfigSettings;

public record GetConfigSettingsQueryResult
{
    public GetConfigSettingsQueryResult(ConfigSetting entity)
    {
        Id = entity.Id;
        Name = entity.Name;
        Value = entity.Value;
        Description = entity.Description;
        Created = entity.Created;
        CreatedBy = entity.CreatedBy;
        LastModified = entity.LastModified;
        LastModifiedBy = entity.LastModifiedBy;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}