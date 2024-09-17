using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.APIClient.Services.AuditLog;
using DPWH.EDMS.Client.Shared.APIClient.Services.DpwhIntegrations;
using DPWH.EDMS.Web.Client.Pages.Home.HomeService;
using DPWH.EDMS.Web.Client.Shared.Services.Document;
using DPWH.EDMS.Web.Client.Shared.Services.Drawing;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandler;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandlerEmployee;
using DPWH.EDMS.Web.Client.Shared.Services.ExceptionHandlerPIS;
using DPWH.EDMS.Web.Client.Shared.Services.Export;
using DPWH.EDMS.Web.Client.Shared.Services.Export.ExportAuditTrail;
using DPWH.EDMS.Web.Client.Shared.Services.FontService;
using DPWH.EDMS.Web.Client.Shared.Services.Navigation;

namespace DPWH.EDMS.Web.Client.Shared.Core;
public static class SharedServiceProvider
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IMenuDataService, MenuDataService>();
        services.AddScoped<IExceptionHandlerService, ExceptionHandlerService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IDpwhIntegrationsService, DpwhIntegrationsService>();
        services.AddScoped<IExceptionPISHandlerService, ExceptionPISHandlerService>();
        services.AddScoped<IExcelExportService, ExcelExportService>();
        services.AddScoped<IExceptionHandlerEmployeeService, ExceptionHandlerEmployeeService>();

        services.AddScoped<IAuditTrailExportService, AuditTrailExportService>();
        services.AddScoped<OverviewFilterService>();
        services.AddScoped<NavRx>();
        services.AddScoped<DrawingService>();
        services.AddScoped<FontServices>();
        return services;
    }
}
