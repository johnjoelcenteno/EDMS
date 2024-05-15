using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application;
using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Api;
using DPWH.EDMS.Api.Middlewares;
using DPWH.EDMS.Api.Swagger;
using DPWH.EDMS.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configManager = ConfigManager.Instance(builder.Configuration);

builder.AddLogging();
builder.Services.AddPresentation(configManager);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configManager);

var app = builder.Build();

app.UseSwagger(configManager);
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.CreateApiVersionSet();
app.UseOutputCache();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapApiEndpoints();
app.Run();