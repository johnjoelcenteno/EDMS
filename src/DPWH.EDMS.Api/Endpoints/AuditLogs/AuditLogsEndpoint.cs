using DPWH.EDMS.Application.Models;
using MediatR;
using KendoNET.DynamicLinq;
using DPWH.EDMS.Application.Features.AuditLogs.Queries.GetAuditLogByEntityId;
using DPWH.EDMS.Application.Features.AuditLogs.Queries.GetAuditLogs;
using Microsoft.AspNetCore.Mvc;
using DPWH.EDMS.Api.Endpoints;

namespace DPWH.EDMS.Api.Endpoints.AuditLogs;

public static class AuditLogsEndpoint
{
    private const string TagName = "AuditLogs";

    public static IEndpointRouteBuilder MapAuditLogs(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.AuditLog.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAuditLogsQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryAuditLog")
            .WithTags(TagName)
            .WithDescription("Retrieve all audit logs.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.AuditLog.Get, async (string id, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAuditLogByEntityIdQuery(id), token);
                var data = new BaseApiResponse<IEnumerable<GetAuditLogByEntityIdResult>>(result);

                return Results.Ok(data);
            })
            .WithName("GetByEntityIdAuditLog")
            .WithTags(TagName)
            .WithDescription("Get by EntityId audit log.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetAuditLogByEntityIdResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
