using DPWH.EDMS.Client.Shared.APIClient.Services.AuditLog;
using DPWH.EDMS.Client.Shared.APIClient.Services.DataLibrary;
using DPWH.EDMS.Client.Shared.APIClient.Services.Licenses;
using DPWH.EDMS.Client.Shared.APIClient.Services.Lookups;
using DPWH.EDMS.Client.Shared.APIClient.Services.Navigation;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordManagement;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordRequestSupportingFiles;
using DPWH.EDMS.Client.Shared.APIClient.Services.RecordTypes;
using DPWH.EDMS.Client.Shared.APIClient.Services.Reports;
using DPWH.EDMS.Client.Shared.APIClient.Services.RequestManagement;
using DPWH.EDMS.Client.Shared.APIClient.Services.Signatories;
using DPWH.EDMS.Client.Shared.APIClient.Services.SystemReport;
using DPWH.EDMS.Client.Shared.APIClient.Services.Users;
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
        services.AddScoped<IRecordRequestSupportingFilesService, RecordRequestSupportingFilesService>();
        services.AddScoped<IRecordManagementService, RecordManagementService>();
        services.AddScoped<IRequestManagementService, RequestManagementService>();
        services.AddScoped<IRecordTypesService, RecordTypesService>();
        services.AddScoped<ISignatoryManagementService, SignatoryManagementService>();
        services.AddScoped<ISystemReportService, SystemReportService>();
        services.AddScoped<IReportsService, ReportsService>();
        services.AddScoped<INavigationService, NavigationService>();
        
        return services;
    }
}