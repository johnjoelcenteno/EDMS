using Blazored.Toast;
using DPWH.EDMS.Client.Shared.Configurations;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Web.Client;
using DPWH.EDMS.Web.Client.Shared.Core;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DPWH.EDMS.Client.Shared.APIClient.Core;
using DPWH.EDMS.Components.Helpers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//make sure the appsetting.json is working or not 
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var config = builder.Configuration.Build();
var configManager = ConfigManager.Instance(config);
builder.Services.AddSingleton(configManager);


// Register the Telerik services.
builder.Services.AddTelerikBlazor();

// add menu or all the static button and nav
builder.Services.AddSharedServices();

// add this for adding Toastr Message prompt
builder.Services.AddBlazoredToast();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// add httpClient using appsettings.json
builder.Services.AddHttpClient(configManager.BaseApiClientName, client =>
{
    client.BaseAddress = new Uri(configManager.BaseApiUrl);
}).AddHttpMessageHandler(sp =>
{
    var handler = sp.GetService<AuthorizationMessageHandler>()!
        .ConfigureHandler(
            authorizedUrls: new[] { configManager.BaseApiUrl },
            scopes: new[] { "movementsoftadminapi.read", "movementsoftadminapi.write" });
    return handler;
}); ;

// Add all api services. This function is for pointing all services in DPWH.NGOBIA.Client.Shared
builder.Services.AddRestApiServices();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin();  //set the allowed origin
        });
});


builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy(nameof(ApplicationPolicies.RequireActiveRoles), policy => policy.RequireRole(ApplicationPolicies.RequireActiveRoles));
    options.AddPolicy(nameof(ApplicationPolicies.AdminOnly), policy => policy.RequireRole(ApplicationPolicies.AdminOnly));
});

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("oidc", options.ProviderOptions);
    options.UserOptions.RoleClaim = "role";
});

await builder.Build().RunAsync();
