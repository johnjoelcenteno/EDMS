namespace DPWH.EDMS.Api;

public static class RecordTypeMappingEndpointExtension
{
    public static IEndpointRouteBuilder MapRecordTypeMappingExtensions(this IEndpointRouteBuilder builder)
    {
        builder.MapRecordTypeMappings();
        return builder;
    }
}
