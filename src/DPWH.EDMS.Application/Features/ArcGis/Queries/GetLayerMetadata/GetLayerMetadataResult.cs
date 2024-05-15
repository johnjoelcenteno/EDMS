using DPWH.EDMS.Application.Features.ArcGis;

namespace DPWH.EDMS.Application.Features.ArcGis.Queries.GetLayerMetadata;

public class GetLayerMetadataResult : ArcGisApiBaseResponse
{
    public decimal CurrentVersion { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? ServiceItemId { get; set; }
    public int CacheMaxAge { get; set; }
    public string? DisplayField { get; set; }
    public string? Description { get; set; }
    public string? CopyrightText { get; set; }
    public bool DefaultVisibility { get; set; }
    public bool IsDataVersioned { get; set; }
    public ArcGisLayerField[]? Fields { get; set; }
    public ArcGisLayerTemplate[]? Templates { get; set; }
}

public class ArcGisLayerField
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? ActualType { get; set; }
    public string? Alias { get; set; }
    public string? SqlType { get; set; }
    public int Length { get; set; }
    public bool Nullable { get; set; }
    public bool Editable { get; set; }
    public object? Domain { get; set; }
    public object? DefaultValue { get; set; }
}

public record ArcGisLayerTemplate
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? DrawingTool { get; set; }
    public Prototype? Prototype { get; set; }
}

public record Prototype
{
    public Dictionary<string, object>? Attributes { get; set; }
}