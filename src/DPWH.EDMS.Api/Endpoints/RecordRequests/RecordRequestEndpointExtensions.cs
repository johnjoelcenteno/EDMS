namespace DPWH.EDMS.Api.Endpoints.RecordRequests;

public static class RecordRequestEndpointExtensions
{
    public static IEndpointRouteBuilder MapRecordRequestsEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapRecordRequests();
        builder.MapRecordRequestSupportingFiles();
        return builder;
    }
}
