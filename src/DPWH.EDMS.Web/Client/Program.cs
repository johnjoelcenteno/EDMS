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
using DPWH.EDMS.Web.Client.BFF;
using Microsoft.AspNetCore.Components.Authorization;
using DPWH.EDMS.Web.Client.Shared.Services.Core;

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

// add service for validation
builder.Services.AddFluentValidatorService();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// Services
builder.Services.AddRestApiServices();

// BFF services
builder.Services.AddTransient<AntiforgeryHandler>();
builder.Services.AddScoped<AccessTokenHandler>();


builder.Services.AddHttpClient(configManager.BaseApiClientName, client =>
{
    client.BaseAddress = new Uri(configManager.BaseApiUrl);
}).AddHttpMessageHandler<AccessTokenHandler>(); 

// Server HTTP Client
builder.Services.AddHttpClient(configManager.WebServerClientName, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<AntiforgeryHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(configManager.WebServerClientName));


// Add all api services. This function is for pointing all services in DPWH.NGOBIA.Client.Shared
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin();
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

builder.Services.AddScoped<AuthenticationStateProvider, BffAuthenticationStateProvider>();

await builder.Build().RunAsync();
