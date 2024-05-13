using DPWH.EDMS.Web.Server.Infrastructure.Extensions;
using DPWH.EDMS.Web.Shared.Configurations;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// setup config
var configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true))
    .Build();
var configManager = ConfigManager.Instance(configuration);
builder.Services.AddSingleton(configManager);

// Add services to the container.
builder.Services.AddBffServices(configManager.Oidc, configuration);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapBffManagementEndpoints();

app.MapRazorPages();
app.MapControllers().RequireAuthorization().AsBffApiEndpoint();
app.MapFallbackToFile("index.html");

app.Run();