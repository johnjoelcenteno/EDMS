using Microsoft.Extensions.Configuration;

namespace DPWH.EDMS.Application.Configurations;

public class ConfigManager
{
    private ConfigManager()
    {
        Security = new SecuritySettings();
        ConnectionStrings = new ConnectionStrings();
        DpwhIntegration = new DpwhIntegrationSettings();
        ArcGisIntegration = new ArcGisIntegrationSettings();
    }

    public static ConfigManager Instance(IConfiguration configuration)
    {
        var instance = new ConfigManager();
        configuration.Bind(instance);

        return instance;
    }

    public SecuritySettings Security { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public bool IsDebug { get; set; }
    public bool EnableSwaggerUI { get; set; }
    public DpwhIntegrationSettings DpwhIntegration { get; set; }
    public ArcGisIntegrationSettings ArcGisIntegration { get; set; }
}

