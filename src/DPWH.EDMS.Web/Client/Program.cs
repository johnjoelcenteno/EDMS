using DPWH.EDMS.Web.Client;
using DPWH.EDMS.Web.Client.Shared.Core;
using DPWH.EDMS.Web.Shared.Configurations;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration.Build();
var configManager = ConfigManager.Instance(config);
builder.Services.AddSingleton(configManager);


builder.Services.AddSharedServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
