namespace DPWH.EDMS.Client.Shared.Models;

public class ConfigSettingModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public string LastModifiedBy { get; set; }
}