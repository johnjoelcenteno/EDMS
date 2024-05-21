﻿using DPWH.EDMS.Client.Shared.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace DPWH.EDMS.Web.Server.Infrastructure.Extensions;

public static class AuthConfigs
{
    public static void AddAuthServices(this IServiceCollection services, OidcConfig oidcSettings, IConfiguration configuration)
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

                options.ClientId = oidcSettings.ClientId;
                options.ClientSecret = string.Empty; 
                options.ResponseType = oidcSettings.ResponseType;

                //options.ReturnUrlParameter = "https://localhost:7232";
                //options.SignedOutRedirectUri = "https://localhost:7232";

                options.Scope.Clear();

                foreach (var scope in oidcSettings.DefaultScopes)
                {
                    options.Scope.Add(scope);
                }

                foreach (var claim in oidcSettings.DefaultClaims)
                {
                    options.ClaimActions.MapUniqueJsonKey(claim, claim);
                }

                options.MapInboundClaims = false;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
            });
    }
}
