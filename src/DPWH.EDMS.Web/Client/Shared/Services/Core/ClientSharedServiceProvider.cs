﻿using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Web.Client.Shared.Services.Navigation;

namespace DPWH.EDMS.Web.Client.Shared.Core;
public static class SharedServiceProvider
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IMenuDataService, MenuDataService>();
        services.AddScoped<IExceptionHandlerService, ExceptionHandlerService>();
        services.AddScoped<NavRx>();

        return services;
    }
}
