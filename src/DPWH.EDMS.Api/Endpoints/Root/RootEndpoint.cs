using DPWH.EDMS.Api.Endpoints;

namespace DPWH.EDMS.Api.Endpoints.Root;

public static class RootEndpoint
{
    private const string Name = "Root";

    public static IEndpointRouteBuilder MapRootEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => TypedResults.Ok('❤'))
            .WithName(Name)
            .WithTags(Name)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .AllowAnonymous();

        app.MapGet("/ip", async (CancellationToken token, ILogger<Program> logger) =>
        {
            logger.LogInformation("Getting ip..");
            var client = new HttpClient();
            var response = await client.GetAsync(@"https://ifconfig.me");
            var responseMessage = await response.Content.ReadAsStringAsync();
            return TypedResults.Ok(responseMessage);
        })
            .WithName("GetIP")
            .WithTags(Name)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .AllowAnonymous();

        return app;
    }
}