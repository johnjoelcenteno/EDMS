using Asp.Versioning.ApiExplorer;
using DPWH.EDMS.Application.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DPWH.EDMS.Api.Swagger;

public class SwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly IHostEnvironment _environment;
    private readonly string _identityServerUrl;

    public SwaggerOptions(IApiVersionDescriptionProvider provider, IHostEnvironment environment, ConfigManager configManager)
    {
        _provider = provider;
        _environment = environment;
        _identityServerUrl = configManager.Security.IdentityServerUrl;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = _environment.ApplicationName,
                    Version = description.ApiVersion.ToString(),
                });
        }

        options.OperationFilter<SwaggerDefaultValues>();
        options.MapType<DateOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date",
            Example = new OpenApiString("2022-02-01")
        });
        options.MapType<TimeOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "time",
            Example = new OpenApiString("21:59:03")
        });

        options.AddSecurityDefinition(
            "oauth2",
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{_identityServerUrl}/connect/authorize"),
                        TokenUrl = new Uri($"{_identityServerUrl}/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {                           
                            { "dpwhedmsapi.write", "api read & write access" }
                        }
                    },

                }
            });

        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
                    },
                    new List<string>()
                }
            });
    }
}