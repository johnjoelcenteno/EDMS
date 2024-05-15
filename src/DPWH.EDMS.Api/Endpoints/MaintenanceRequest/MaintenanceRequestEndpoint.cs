using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Features.Maintenance.Commands.CreateMaintenanceRequest;
using DPWH.EDMS.Application.Features.Maintenance.Commands.UpdateMaintenanceRequest;
using DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequestByBuildingId;
using DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequestById;
using DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequestNumberByBuildingId;
using DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequests;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.MaintenanceRequest;

public static class MaintenanceRequestEndpoint
{
    public const string TagName = "MaintenanceRequest";

    public static IEndpointRouteBuilder MapMaintenanceRequest(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Maintenance.Get, async ([FromRoute] Guid id, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetMaintenanceRequestByIdQuery(id);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<GetMaintenanceRequestByIdResult>(response);

            return result.Data == null ? Results.NotFound("Maintenance request not found") : Results.Ok(result);
        })
            .WithName("GetMaintenanceRequestById")
            .WithTags(TagName)
            .WithDescription("Get maintenance request by id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetMaintenanceRequestByIdResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.Maintenance.GetByBuildingId, async (string buildingId, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetMaintenanceRequestByBuildingIdQuery(buildingId);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<GetMaintenanceRequestByIdResult>(response);

            return result.Data == null ? Results.NotFound("Maintenance request not found") : Results.Ok(result);
        })
            .WithName("GetMaintenanceRequestByBuildingId")
            .WithTags(TagName)
            .WithDescription("Get maintenance request by building id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetMaintenanceRequestByIdResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.Maintenance.GetMaintenanceNumbersByBuildingId, async (string buildingId, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetMaintenanceRequestNumberByBuildingIdQuery(buildingId);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<GetMaintenanceRequestNumberByBuildingIdResult>(response);

            return result.Data == null ? Results.NotFound("Maintenance request not found") : Results.Ok(result);
        })
            .WithName("GetMaintenanceRequestNumberByBuildingId")
            .WithTags(TagName)
            .WithDescription("Get maintenance request numbers by building id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetMaintenanceRequestNumberByBuildingIdResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.Maintenance.Create, async (CreateMaintenanceRequestCommand request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(request, token);
            var data = new BaseApiResponse<Guid>(result);

            return Results.Ok(data);
        })
            .WithName("CreateMaintenanceRequest")
            .WithTags(TagName)
            .WithDescription("Create new maintenance request.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Guid>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.Maintenance.Update, async (UpdateMaintenanceRequestCommand request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(request, token);
            var data = new BaseApiResponse<UpdateMaintenanceRequestResult>(result);

            return Results.Ok(data);
        })
            .WithName("UpdateMaintenanceRequest")
            .WithTags(TagName)
            .WithDescription("Update maintenance request.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateMaintenanceRequestResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.Maintenance.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new GetMaintenanceRequest(request), token);

            return Results.Ok(result);
        })
            .WithName("QueryMaintenanceRequests")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
