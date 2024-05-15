namespace DPWH.EDMS.Application.Features.ArcGis.Service;

public interface IArcGisTokenProvider
{
    Task<ArcGisAccessToken> GetAccessToken(CancellationToken cancellationToken = default);
}