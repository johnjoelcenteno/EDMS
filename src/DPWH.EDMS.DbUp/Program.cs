using DbUp;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Reflection;

var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

var connectionString = config.GetConnectionString("DefaultConnection");

var upgrader = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .WithTransactionPerScript()
    .LogTo(new SerilogUpgradeLogger(Log.Logger))
    .Build();

EnsureDatabase.For.SqlDatabase(connectionString);

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
#if DEBUG
    Console.ReadLine();
#endif
    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();
Log.Logger.Information("Successfully completed database upgraded.");
Log.CloseAndFlush();

return 0;