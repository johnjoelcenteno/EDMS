namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetBuildingComponents;

public record GetBuildingComponentsResult
{
    public string Id { get; set; }
    public int? Ordinal { get; set; }
    public string? ItemNo { get; set; }
    public string Name { get; set; }
    public IEnumerable<GetBuildingComponentsResultCategory> Categories { get; set; }
}

public record GetBuildingComponentsResultCategory
{
    public string Id { get; set; }
    public int? Ordinal { get; set; }
    public string? ItemNo { get; set; }
    public string Name { get; set; }
    public IEnumerable<GetBuildingComponentsResultSubcategory> Subcategories { get; set; }
}

public record GetBuildingComponentsResultSubcategory
{
    public string Id { get; set; }
    public int? Ordinal { get; set; }
    public string Name { get; set; }
    public IEnumerable<GetBuildingComponentsResultItems> Items { get; set; }
}

public record GetBuildingComponentsResultItems
{
    public string? ItemId { get; set; }
    public string? ItemNo { get; set; }
    public string? Suffix { get; set; }
    public string Description { get; set; }
    public string? Thickness { get; set; }
    public string? Class { get; set; }
    public string? Others { get; set; }
    public string? UnitOfMeasure { get; set; }
}