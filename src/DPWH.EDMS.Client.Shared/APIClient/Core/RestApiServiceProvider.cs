using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequests;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;
using DPWH.NGOBIA.Client.Shared.APIClient.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DPWH.EDMS.Client.Shared.APIClient.Core;

public static class RestApiServiceProvider
{
    public static IServiceCollection AddRestApiServices(this IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<ILookupsService, LookupsService>();
        services.AddScoped<ILicensesService, LicensesService>();
        services.AddScoped<IDataLibraryService, DataLibraryService>();
        services.AddScoped<IRecordRequestsService, RecordRequestsService>();
        services.AddScoped<IRecordRequestSupportingFilesService, RecordRequestSupportingFilesService>();

        return services;
    }
}