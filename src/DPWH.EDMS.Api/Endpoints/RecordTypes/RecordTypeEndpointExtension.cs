namespace DPWH.EDMS.Api;

public static class RecordTypeEndpointExtension
{
    public static IEndpointRouteBuilder MapRecordTypeExtensions(this IEndpointRouteBuilder builder)
    {
        builder.MapRecordTypes();
        return builder;
    }
}
