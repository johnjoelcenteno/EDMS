using DPWH.EDMS.Api.Endpoints.ProjectMonitoring;
using DPWH.EDMS.Api.Endpoints.Addresses;
using DPWH.EDMS.Api.Endpoints.Licenses;
using DPWH.EDMS.Api.Endpoints.SystemLogs;
using DPWH.EDMS.Api.Endpoints.DpwhIntegrations;
using DPWH.EDMS.Api.Endpoints.Lookups;
using DPWH.EDMS.Api.Endpoints.Inspections;
using DPWH.EDMS.Api.Endpoints.DataLibraries;
using DPWH.EDMS.Api.Endpoints.DataSync;
using DPWH.EDMS.Api.Endpoints.Roles;
using DPWH.EDMS.Api.Endpoints.ArcGisIntegrations;
using DPWH.EDMS.Api.Endpoints.MaintenanceRequest;
using DPWH.EDMS.Api.Endpoints.AuditLogs;
using DPWH.EDMS.Api.Endpoints.Root;
using DPWH.EDMS.Api.Endpoints.Users;
using DPWH.EDMS.Api.Endpoints.Assets;
using DPWH.EDMS.Api.Endpoints.Reports;
using DPWH.EDMS.Api.Endpoints.RecordRequests;

namespace DPWH.EDMS.Api.Endpoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        // new endpoints
        app.MapRecordRequestsEndpoints();
        app.MapEmployeeRequestEndpointExtensions();        

        app.MapRootEndpoints();
        //app.MapAssetEndpoints();
        app.MapLookupsEndpoints();
        app.MapAddressEndpoints();
        app.MapLicensesEndpoints();
        app.MapUsersEndpoints();
        app.MapRoleEndpoints();
        //app.MapReportsEndpoints();
        app.MapAuditLogsEndpoints();
        app.MapSystemLogsEndpoints();
        app.MapDpwhIntegrationsEndpoints();
        //app.MapArcGisIntegrationsEndpoints();
        app.MapDataLibrariesEndpoints();
        //app.MapDataSyncEndpoints();
        //app.MapInspectionsEndpoints();
        //app.MapMaintenanceRequestEndpoints();
        //app.MapProjectMonitoringEndpoints();

        return app;
    }
}