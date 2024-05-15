using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssetsByBuildingId;
using DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionProjectMonitoringMonthly;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.CreateProjectMonitoring;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.UpdateProjectMonitoring;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetAssetApprovedPriorityListing;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetContractIdsByBuildingId;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetProjectMonitoring;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetProjectMonitoringByBuildingId;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetProjectMonitoringById;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.ProjectMonitoring;

public static class ProjectMonitoringEndpoint
{
    public const string TagName = "ProjectMonitoring";

    public static IEndpointRouteBuilder MapProjectMonitoring(this IEndpointRouteBuilder app)
    {

        app.MapPost(ApiEndpoints.ProjectMonitoring.Create, async (CreateProjectMonitoringCommand request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(request, token);
            var data = new BaseApiResponse<Guid>(result);

            return Results.Ok(data);
        })
        .WithName("CreateProjectMonitoring")
        .WithTags(TagName)
        .WithDescription("Create new project Monitoring.")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Guid>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.ProjectMonitoring.Update, async (UpdateProjectMonitoringCommand request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(request, token);
            var data = new BaseApiResponse<Guid>(result);

            return Results.Ok(data);
        })
        .WithName("UpdateProjectMonitoring")
        .WithTags(TagName)
        .WithDescription("Update project monitoring.")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Guid>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.ProjectMonitoring.Get, async ([FromRoute] Guid id, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetProjectMonitoringByIdQuery(id);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<GetProjectMonitoringByIdResult>(response);

            return result.Data == null ? Results.NotFound("Project Monitoring not found") : Results.Ok(result);
        })
           .WithName("GetProjectMonitoringById")
           .WithTags(TagName)
           .WithDescription("Get project monitoring by id.")
           .WithApiVersionSet(ApiVersioning.VersionSet)
           .HasApiVersion(1.0)
           .Produces<BaseApiResponse<GetProjectMonitoringByIdResult>>()
           .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
           .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.ProjectMonitoring.GetByBuildingId, async (string buildingId, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetProjectMonitoringByBuildingIdQuery(buildingId);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<GetProjectMonitoringByIdResult>(response);

            return result.Data == null ? Results.NotFound("Project Monitoring not found") : Results.Ok(result);
        })
            .WithName("GetProjectMonitoringByBuildingId")
            .WithTags(TagName)
            .WithDescription("Get project monitoring by building id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetProjectMonitoringByIdResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.ProjectMonitoring.GetAssetApprovedInspection, async (string buildingId, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetAssetApprovedPriorityListingQuery(buildingId);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<GetAssetsByBuildingIdResult>(response);

            return result.Data == null ? Results.NotFound("Asset not found") : Results.Ok(result);
        })
            .WithName("GetAssetApprovedPriorityListByBuildingId")
            .WithTags(TagName)
            .WithDescription("Get asset with approved IR Priority Listing by building id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetAssetsByBuildingIdResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.ProjectMonitoring.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new GetProjectMonitoringQuery(request), token);

            return Results.Ok(result);
        })
            .WithName("QueryProjectMaintenance")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.ProjectMonitoring.GetProjectMonitoringMonthlyById, async ([FromRoute] Guid projectMonitoringId, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetInspectionProjectMonitoringMonthlyQuery(projectMonitoringId);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<GetInspectionProjectMonitoringMonthlyResult>(response);

            return result.Data == null ? Results.NotFound("Building components not found") : Results.Ok(result);
        })
            .WithName("GetProjectMonitoringMonthlyById")
            .WithTags(TagName)
            .WithDescription("Get project monitoring monthly by project monitoring id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetInspectionProjectMonitoringMonthlyResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);


        app.MapGet(ApiEndpoints.ProjectMonitoring.GetContractIdsByBuildingId, async ([FromRoute] string buildingId, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetContractIdsByBuildingIdQuery(buildingId);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<GetContractIdByBuildingIdResult>(response);

            return result.Data == null ? Results.NotFound("Contract Ids not found") : Results.Ok(result);
        })
            .WithName("GetProjectMonitoringContractIdsByBuildingId")
            .WithTags(TagName)
            .WithDescription("Get project monitoring contract ids by building id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetContractIdByBuildingIdResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
