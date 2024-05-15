namespace DPWH.EDMS.Api.Endpoints.Addresses;

public static class AddressesEndpointExtensions
{
    public static IEndpointRouteBuilder MapAddressEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAddresses();
        return app;
    }
}
