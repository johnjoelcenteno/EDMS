namespace DPWH.EDMS.Application.Features.ArcGis;

public abstract class ArcGisApiBaseResponse
{
    public ArcGisError? Error { get; set; }
}

public record ArcGisError
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public string[]? Details { get; set; }
}