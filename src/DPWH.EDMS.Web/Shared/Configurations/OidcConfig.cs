namespace DPWH.EDMS.Web.Shared.Configurations;

public class OidcConfig
{
    public string Authority { get; set; }
    public string ClientId { get; set; }
    public string[] DefaultScopes { get; set; }
    public string RedirectUri { get; set; }
}