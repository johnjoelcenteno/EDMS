namespace DPWH.EDMS.Api.Endpoints.Navigation;

public static class NavigationEndpointExtension
{
    public static IEndpointRouteBuilder MapNavigationExtensions(this IEndpointRouteBuilder builder)
    {
        builder.MapNavigation();
        return builder;
    }
}
