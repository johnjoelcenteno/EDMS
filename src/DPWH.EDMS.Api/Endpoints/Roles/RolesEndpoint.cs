using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Features.Roles.Queries.GetRoles;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Roles;

public static class RolesEndpoint
{
    private const string TagName = "Roles";

    public static IEndpointRouteBuilder MapRoles(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Roles.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetRolesQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryRoles")
            .WithTags(TagName)
            .WithDescription("Get roles in the system.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
