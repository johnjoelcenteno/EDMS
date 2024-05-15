using DPWH.EDMS.Infrastructure.Context;
using DPWH.EDMS.Infrastructure.Services;
using DPWH.EDMS.Infrastructure.Storage.Services;
using DPWH.EDMS.Application.Configurations;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.IDP.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DPWH.EDMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigManager configManager)
    {
        services.AddDatabases(configManager);
        services.AddStorages(configManager);

        services.AddScoped<IDpwhApiService, DpwhApiService>();

        return services;
    }

    private static IServiceCollection AddDatabases(this IServiceCollection services, ConfigManager configManager)
    {
        services.AddScoped<AuditInterceptor>();
        services.AddDbContext<AppDataContext>((provider, options) =>
        {
            options.UseSqlServer(configManager.ConnectionStrings.DefaultConnection);
            options.AddInterceptors(provider.GetRequiredService<AuditInterceptor>());
        });
        services.AddScoped<IReadRepository>(p => p.GetService<AppDataContext>()!);
        services.AddScoped<IWriteRepository>(p => p.GetService<AppDataContext>()!);

        services.AddDbContext<AppIdpDataContext>(options =>
        {
            options.UseSqlServer(configManager.ConnectionStrings.IdentityConnection);
        });
        services.AddScoped<IReadAppIdpRepository>(p => p.GetService<AppIdpDataContext>()!);

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppIdpDataContext>();
        services.TryAddScoped<UserManager<ApplicationUser>>();
        services.TryAddScoped<RoleManager<IdentityRole>>();

        return services;
    }

    private static IServiceCollection AddStorages(this IServiceCollection services, ConfigManager configManager)
    {
        ArgumentNullException.ThrowIfNull(configManager);

        var conn = configManager.ConnectionStrings;

        services.AddSingleton<ITableService>(c => new TableService(conn.StorageAccountConnection));
        services.AddSingleton<IBlobService>(c => new BlobService(conn.StorageAccountConnection));
        services.AddScoped<IQueueService>(c => new QueueService(conn.StorageAccountConnection));
        services.AddScoped<INotificationQueueService>(c => new NotificationQueueService(conn.NotificationStorageAccountConnection));

        return services;
    }
}
