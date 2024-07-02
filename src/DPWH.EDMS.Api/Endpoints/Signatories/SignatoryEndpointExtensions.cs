namespace DPWH.EDMS.Api;

public static class SignatoryEndpointExtensions
{
    public static IEndpointRouteBuilder MapSignatoryEndpointExtensions(this IEndpointRouteBuilder builder)
    {
        builder.MapSignatoryEndpoints();
        return builder;
    }
}
