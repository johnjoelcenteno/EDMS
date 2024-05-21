using DPWH.EDMS.Api.Swagger;
using DPWH.EDMS.Application.Configurations;

namespace DPWH.EDMS.Api.Swagger;

public static class SwaggerExtensionBuilder
{
    public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, ConfigManager config)
    {
        if (!config.EnableSwaggerUI)
        {
            return app;
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var name in ((WebApplication)app).DescribeApiVersions().Select(d => d.GroupName))
            {
                options.SwaggerEndpoint($"/swagger/{name}/swagger.json", name);
                options.OAuthClientId("dpwhedmsapiswagger");
                options.OAuthScopes("profile", "openid", "dpwhedms", "dpwhedmsapi.read", "dpwhedmsapi.write");
                options.OAuthUsePkce();
                options.InjectStylesheet("/assets/css/theme-flattop.css");
                options.DisplayOperationId();
            }
        });

        return app;
    }
}