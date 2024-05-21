namespace DPWH.EDMS.Client.Shared.Configurations;

public class OidcConfig
{
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string[] DefaultScopes { get; set; }
    public string[] DefaultClaims { get; set; }
    public string RedirectUri { get; set; }
    public string ResponseType { get; set; }
}