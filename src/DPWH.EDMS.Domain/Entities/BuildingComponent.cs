using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class BuildingComponent : EntityBase
{
    public string CategoryId { get; set; }
    public int Ordinal { get; set; }
    public string? ItemNo { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string? ParentId { get; set; }
    public string? Suffix { get; set; }
    public string? Description { get; set; }
    public string? Thickness { get; set; }
    public string? Class { get; set; }
    public string? Others { get; set; }
    public string? UnitOfMeasure { get; set; }
}