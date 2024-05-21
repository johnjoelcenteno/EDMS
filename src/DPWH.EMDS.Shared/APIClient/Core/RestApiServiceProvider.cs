using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DPWH.EDMS.Client.Shared.APIClient.Core;

public static class RestApiServiceProvider
{
    public static IServiceCollection AddRestApiServices(this IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<ILookupsService, LookupsService>();
        return services;
    }
}