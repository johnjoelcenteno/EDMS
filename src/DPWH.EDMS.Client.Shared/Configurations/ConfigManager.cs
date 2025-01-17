using Microsoft.Extensions.Configuration;

namespace DPWH.EDMS.Client.Shared.Configurations;

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
    public string WebServerClientName { get; set; }
    public string BaseApiClientName { get; set; }
    public string BaseApiUrl { get; set; }
    public string PublicPortalUrl { get; set; }
    public string[] PropertyStatus { get; set; }
    
    public List<string> SectionDataLibrary { get; set; }
    public List<string> OfficeDataLibrary { get; set; }
    public List<string> PublicUrls { get; set; }
    public List<string> StatusList { get; set; }
    public List<string> ReportType { get; set; }
    public List<string> DataLibraryType { get; set; }
    public List<string> ExcludedColumns { get; set; }
}