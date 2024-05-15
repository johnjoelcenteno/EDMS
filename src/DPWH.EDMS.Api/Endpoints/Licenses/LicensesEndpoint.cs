using MediatR;
using DPWH.EDMS.Application.Features.Licenses.Queries;
using DPWH.EDMS.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Licenses;

public static class LicensesEndpoint
{
    private const string TagName = "Licenses";

    public static IEndpointRouteBuilder MapLicenses(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Licenses.Status, async (IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetLicenseStatusQuery(), token);
                var data = new BaseApiResponse<GetLicenseStatusResult>(result);

                return Results.Ok(data);
            })
            .WithName("GetLicenseStatus")
            .WithTags(TagName)
            .WithDescription("Get license status. This shows how many licenses are available or consume.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetLicenseStatusResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}