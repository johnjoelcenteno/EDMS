using DPWH.EDMS.Application.Features.Systems.Commands;
using DPWH.EDMS.Application.Features.Systems.Queries.GetSystemLogById;
using DPWH.EDMS.Application.Features.Systems.Queries.GetSystemLogs;
using DPWH.EDMS.Application.Features.Systems.Queries.Models;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;

namespace DPWH.EDMS.Api.Endpoints.SystemLogs;

public static class SystemLogsEndpoint
{
    private const string TagName = "System";

    public static IEndpointRouteBuilder MapSystemLogs(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.System.Create, async (CreateSystemLogsRequest request, IMediator mediator, CancellationToken token) =>
            {
                var response = await mediator.Send(new Create<CreateSystemLogsRequest, CreateResponse>(request), token);
                return TypedResults.Ok(response);
            })
            .WithName("CreateSystemLogs")
            .WithTags(TagName)
            .WithDescription("Create System Update.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<CreateResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);

        app.MapGet(ApiEndpoints.System.Get, async (Guid id, IMediator mediator, CancellationToken token) =>
            {
                var response = await mediator.Send(new GetSystemLogByIdQuery(id), token);
                return Results.Ok(response);
            })
            .WithName("GetSystemLog")
            .WithTags(TagName)
            .WithDescription("Get System Log.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<SystemLogsResponse>(StatusCodes.Status200OK)
            .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);

        app.MapPost(ApiEndpoints.System.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetSystemLogsQuery(request), token);

                return result;
            })
            .WithName("QuerySystemLogs")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPut(ApiEndpoints.System.Update, async (Guid id, UpdateSystemLogsRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new UpdateWithId<UpdateSystemLogsRequest, UpdateResponse>(id, request), token);

                return result;
            })
            .WithName("UpdateSystemLog")
            .WithTags(TagName)
            .WithDescription("Update System Log Update")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<UpdateResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapDelete(ApiEndpoints.System.Delete, async (Guid id, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new DeleteWithId<DeleteSystemLogsRequest, DeleteResponse>(id), token);
                return result;
            })
            .WithName("DeleteSystemLog")
            .WithTags(TagName)
            .WithDescription("Delete system log")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DeleteResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}