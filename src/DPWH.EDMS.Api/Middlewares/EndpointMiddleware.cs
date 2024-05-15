using Microsoft.AspNetCore.Authorization;

namespace DPWH.EDMS.Api.Middlewares;

public static class EndpointMiddleware
{
    public static void UseCustomEndpointRouting(this IApplicationBuilder app, IHostEnvironment env, bool enableAuthInLocalDev)
    {
        app.UseEndpoints(endpoints =>
        {
            var isDevelopment = env.IsDevelopment() && !enableAuthInLocalDev;

            // If we're running in local development then authentication gets in the way, so the following
            // overrides all controller authorisation, allowing for anonymous access, but only in local dev.
            // If you want your controller requests to be authenticated in local dev, remove this statement.
            if (isDevelopment)
            {
                endpoints.MapControllers().WithMetadata(new AllowAnonymousAttribute());
            }
            else
            {
                endpoints.MapControllers().RequireAuthorization();
            }
        });
    }
}
