using DPWH.EDMS.Api.Endpoints.RecordRequests;

namespace DPWH.EDMS.Api;

public static class RecordRequestEndpointExtensions
{
    public static IEndpointRouteBuilder MapRecordRequestsEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapRecordRequests();
        builder.MapRecordRequestSupportingFiles();
        return builder;
    }
}
