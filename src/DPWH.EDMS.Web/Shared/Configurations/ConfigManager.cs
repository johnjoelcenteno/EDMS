using Microsoft.Extensions.Configuration;

namespace DPWH.EDMS.Web.Shared.Configurations;

public class ConfigManager
{
    private ConfigManager()
    {
    }
    
    public static ConfigManager Instance(IConfiguration configuration)
    {
        var instance = new ConfigManager();
        configuration.Bind(instance);
        return instance;
    }

    public OidcConfig Oidc { get; set; }
    public string BaseApiClientName { get; set; }
    public string BaseApiUrl { get; set; }
    public string PublicPortalUrl { get; set; }
    public string[] PropertyStatus { get; set; }

}