using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class ConfigSetting : EntityBase
{
    private ConfigSetting(string name, string value, string? description, string createdBy)
    {
        Name = name;
        Value = value;
        Description = description;

        SetCreated(createdBy);
    }

    public static ConfigSetting Create(string name, string value, string? description, string createdBy)
    {
        return new ConfigSetting(name, value, description, createdBy);
    }

    public void UpdateDetails(string name, string value, string? description, string modifiedBy)
    {
        Name = name;
        Value = value;
        Description = description;

        SetModified(modifiedBy);
    }

    public string Name { get; private set; }
    public string Value { get; private set; }
    public string? Description { get; private set; }
}
