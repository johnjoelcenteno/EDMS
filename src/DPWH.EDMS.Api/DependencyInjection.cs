using System.Security.Claims;
using System.Text.Json.Serialization;
using Asp.Versioning;
using DPWH.EDMS.Api;
using DPWH.EDMS.Api.Swagger;
using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Application.Converters;
using DPWH.EDMS.Application.Extensions;
using DPWH.EDMS.IDP.Core.Constants;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DPWH.EDMS.Api;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .BuildLoggerFromConfiguration(builder.Configuration, typeof(Program));

        builder.Host.UseSerilog();

        return builder;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigManager configManager)
    {
        services.AddSingleton(configManager);
        services.AddAuthentication(configManager.Security);
        services.AddAuthorization(configManager.Security);

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1.0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
        }).AddApiExplorer();

        services.AddEndpointsApiExplorer();
        services.AddProblemDetails();

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptions>();
        services.AddSwaggerGen();

        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new ObjectToInferredTypesConverter()));

        services.AddOutputCache();
        services.AddHttpContextAccessor();
        services.AddTransient(provider => provider.GetService<IHttpContextAccessor>()!.HttpContext!.User);

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, SecuritySettings securitySettings)
    {
        ArgumentNullException.ThrowIfNull(securitySettings);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = securitySettings.IdentityServerUrl;
                options.Audience = "dpwhedmsapi";
                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                // if token does not contain a dot, it is a reference token
                options.ForwardDefaultSelector = ForwardReferenceToken();
            })//.AddCookie()
            .AddOAuth2Introspection("introspection", options =>
            {
                options.Authority = securitySettings.IdentityServerUrl;
                options.ClientId = "dpwhedmsapi";
                options.ClientSecret = "dpwhedmsapisecret";
            });
        return services;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services, SecuritySettings securitySettings)
    {
        ArgumentNullException.ThrowIfNull(securitySettings);

        services.AddAuthorization(options =>
        {
            foreach (var item in ApplicationPolicies.GetAll)
            {
                options.AddPolicy(item.Key, policy => policy.RequireClaim(JwtClaimTypes.Role, item.Value));
            }

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireClaim(JwtClaimTypes.Role, ApplicationPolicies.RequireActiveRoles)
                .Build();
        });

        services.AddHsts(options => options.MaxAge = TimeSpan.FromDays(120));

        services.AddCors(options =>
        {
            var origins = securitySettings.CorsUrls.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();

            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    /// <summary>
    /// Provides a forwarding func for JWT vs reference tokens (based on existence of dot in token)
    /// </summary>
    /// <param name="introspectionScheme">Scheme name of the introspection handler</param>
    /// <returns></returns>
    private static Func<HttpContext, string?> ForwardReferenceToken(string? introspectionScheme = "introspection")
    {
        string? Select(HttpContext context)
        {
            var (scheme, credential) = GetSchemeAndCredential(context);

            if (scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase) && !credential.Contains("."))
            {
                return introspectionScheme;
            }

            return null;
        }

        return Select;
    }

    /// <summary>
    /// Extracts scheme and credential from Authorization header (if present)
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private static (string, string) GetSchemeAndCredential(HttpContext context)
    {
        var header = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(header))
        {
            return ("", "");
        }

        var parts = header.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            return ("", "");
        }

        return (parts[0], parts[1]);
    }
}