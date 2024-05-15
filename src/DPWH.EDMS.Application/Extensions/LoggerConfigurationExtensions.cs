using System.Reflection;
using DPWH.EDMS.Domain.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace DPWH.EDMS.Application.Extensions;

public static class LoggerConfigurationExtensions
{
    public static ILogger BuildLoggerFromConfiguration(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, Type? startupType = null)
    {
        var environment = configuration.GetValue<string>("Environment") ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var instrumentationKey = configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY") ?? "";
        var isAppInsightsConfigured = !string.IsNullOrEmpty(instrumentationKey);

        var isConsoleConfigured = environment == "Development" || configuration.GetValue<bool>("ConsoleLogging");

        var assembly = startupType?.Assembly ?? Assembly.GetEntryAssembly() ?? throw new NullReferenceException();
        var assemblyName = assembly.GetName().Name;
        var assemblyVersion = assembly.GetName().Version;

        if (configuration is ConfigurationRoot configurationRoot)
        {
            foreach (var configurationProvider in configurationRoot.Providers)
            {
                Console.WriteLine($"Configuration Provider: {configurationProvider.GetType().Name}");
            }
        }

        var levelSwitch = new LoggingLevelSwitch { MinimumLevel = LogEventLevel.Information };

        loggerConfiguration
            .MinimumLevel.ControlledBy(levelSwitch)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", assemblyName)
            .Enrich.WithProperty("ApplicationVersion", assemblyVersion)
            .Enrich.WithProperty("Environment", environment)
            .If(isConsoleConfigured, c => c.WriteTo.Console())
            .If(isAppInsightsConfigured,
                c => c.WriteTo.ApplicationInsights(new TelemetryConfiguration(instrumentationKey), TelemetryConverter.Traces));
        //c => c.WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces));

        var logger = loggerConfiguration.CreateLogger();

        logger.Information("startupType?.BaseType?.Name = {StartupType}", startupType?.BaseType?.BaseType?.Name);
        logger.Information("environment = {Environment}", environment);
        logger.Information("isAppInsightsConfigured = {IsAppInsightsConfigured}", isAppInsightsConfigured);
        logger.Information("instrumentationKey = {InstrumentationKey}", instrumentationKey);
        logger.Information("isConsoleConfigured = {IsConsoleConfigured}", isConsoleConfigured);
        logger.Information("applicationName = {ApplicationName}", assemblyName);
        logger.Information("applicationVersion = {ApplicationVersion}", assemblyVersion);

        return logger;
    }
}