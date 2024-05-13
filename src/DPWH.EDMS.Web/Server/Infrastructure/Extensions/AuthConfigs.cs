using DPWH.EDMS.Web.Shared.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace DPWH.EDMS.Web.Server.Infrastructure.Extensions;

public static class AuthConfigs
{
    public static void AddBffServices(this IServiceCollection services, OidcConfig oidcSettings, IConfiguration configuration)
    {
        services.AddBff();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "cookie";
            options.DefaultChallengeScheme = "oidc";
            options.DefaultSignOutScheme = "oidc";
        })
            .AddCookie("cookie", options =>
            {
                options.Cookie.Name = "__Host-blazor";
                options.Cookie.SameSite = SameSiteMode.Strict;
            })
            .AddOpenIdConnect("oidc", options =>
            {
                configuration.Bind("oidc", options);
                options.Authority = oidcSettings.Authority;

                options.ClientId = "bookingportal";
                options.ClientSecret = "bookingportalsecret";
                options.ResponseType = "code";
                options.ResponseMode = "query";

                //options.ReturnUrlParameter = "https://localhost:7232";
                //options.SignedOutRedirectUri = "https://localhost:7232";

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("bookingplatform");
                options.Scope.Add("bookingplatformapi.read");
                options.Scope.Add("bookingplatformapi.write");

                // Customize OIDC options
                options.ClaimActions.MapUniqueJsonKey("role", "role");

                // Configure the ClaimsPrincipalFactory
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };

                options.MapInboundClaims = false;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
            });
        //.AddIdentityCookies();
    }
}
