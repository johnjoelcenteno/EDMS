namespace DPWH.EDMS.Application.Features.ArcGis.Service;

public record ArcGisAccessToken
{
    public string? Token { get; set; }
    public long Expires { get; set; }
    public bool Ssl { get; set; }
}