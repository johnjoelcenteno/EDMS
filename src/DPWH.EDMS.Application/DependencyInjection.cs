﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Behaviours;
using DPWH.EDMS.Application.Services;

namespace DPWH.EDMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ICoreApplicationMarker).Assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddHttpClient();
        services.AddValidatorsFromAssemblyContaining<ICoreApplicationMarker>(ServiceLifetime.Singleton);
        
        services.AddScoped<IUserAccessLevelService, UserAccessLevelService>();
        
        services.AddScoped<IBuildingIdSequenceGeneratorService, BuildingIdSequenceGeneratorService>();
        services.AddScoped<IControlNumberGeneratorService, ControlNumberGeneratorService>();
        services.AddScoped<IRequestNumberGeneratorService, RequestNumberGeneratorService>();        
        services.AddScoped<IRentalRateNumberGeneratorService, RentalRateNumberGeneratorService>();

        return services;
    }
}