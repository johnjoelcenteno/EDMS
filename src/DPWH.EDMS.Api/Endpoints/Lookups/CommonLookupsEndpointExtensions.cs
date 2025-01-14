﻿namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class CommonLookupsEndpointExtensions
{
    public const string Tag = "Lookups";

    public static IEndpointRouteBuilder MapLookupsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapLookups();
        app.MapGeoLocations();
        app.MapAgencies();
        app.MapRequestingOfficeLookups();
        
        app.MapRecordTypes();
        app.MapValidIDs();        
        app.MapAuthorizationDocuments();
        app.MapPurposes();

        return app;
    }
}