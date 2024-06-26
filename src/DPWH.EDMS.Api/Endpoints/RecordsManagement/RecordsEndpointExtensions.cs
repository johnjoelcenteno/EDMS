namespace DPWH.EDMS.Api.Endpoints.RecordsManagement;

public static class RecordEndpointExtensions
{
    public static IEndpointRouteBuilder MapRecordsEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapRecords();
        return builder;
    }
}
