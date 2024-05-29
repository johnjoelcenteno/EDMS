namespace DPWH.EDMS.Api;

public static class EmployeeRequestEndpointExtensions
{
    public static IEndpointRouteBuilder MapEmployeeRequestEndpointExtensions(this IEndpointRouteBuilder builder)
    {
        builder.MapEmployeeRecordEndpoint();
        return builder;
    }
}
