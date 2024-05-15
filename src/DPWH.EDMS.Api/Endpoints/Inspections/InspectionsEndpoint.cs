using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssetsByBuildingId;
using DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequest;
using DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.DeleteBuildingComponent;
using DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateBuildingComponent;
using DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateInspectionRequest;
using DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateProjectMonitoring;
using DPWH.EDMS.Application.Features.Inspections.Queries.GetInspectionRequests;
using DPWH.EDMS.Application.Features.Inspections.Queries.GetInspectorById;
using DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetAssetByBuildingId;
using DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetBuildingComponentsByRequestNumber;
using DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionProjectMonitoringMonthly;
using DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetInspectionRequestById;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Inspections;

public static class InspectionsEndpoint
{
    public const string TagName = "InspectionRequest";

    public static IEndpointRouteBuilder MapInspections(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Inspection.GetInspectorById, async (string employeeId, IMediator mediator, CancellationToken token) =>
            {
                var request = new GetInspectorByIdQuery(employeeId);
                var response = await mediator.Send(request, token);

                var result = new BaseApiResponse<GetInspectorByIdResult>(response);

                return result.Data == null ? Results.NotFound("Inspector not found") : Results.Ok(result);
            })
            .WithName("GetInspectorByEmployeeId")
            .WithTags(TagName)
            .WithDescription("Get inspector by employee id")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetInspectorByIdResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.Inspection.Get, async ([FromRoute] Guid id, IMediator mediator, CancellationToken token) =>
            {
                var request = new GetInspectionRequestByIdCommand(id);
                var response = await mediator.Send(request, token);

                var result = new BaseApiResponse<GetInspectionRequestByIdResult>(response);

                return result.Data == null ? Results.NotFound("Inspection request not found") : Results.Ok(result);
            })
            .WithName("GetInspectionRequestById")
            .WithTags(TagName)
            .WithDescription("Get inspector inspection request by id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetInspectionRequestByIdResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.Inspection.GetAssetByBuildingId, async ([FromRoute] string buildingId, string? purpose, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetAssetByBuildingIdQuery(buildingId, purpose);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<GetAssetByBuildingIdWithBuildingComponentResult>(response);

            return result.Data == null ? Results.NotFound("Asset not found") : Results.Ok(result);
        })
            .WithName("GetAssetByBuildingIdWithBuildingComponents")
            .WithTags(TagName)
            .WithDescription("Get asset by building Id for Priority List Inspection / Project Monitoring request by building id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetAssetByBuildingIdWithBuildingComponentResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.Inspection.GetBuildingComponentsByRequestNumber, async ([FromRoute] string requestNumber, string purpose, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetBuildingComponentsByRequestNumberQuery(requestNumber, purpose);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<GetBuildingComponentsByRequestNumberResult>(response);

            return result.Data == null ? Results.NotFound("Building components not found") : Results.Ok(result);
        })
            .WithName("GetBuildingComponentsByRequestNumber")
            .WithTags(TagName)
            .WithDescription("Get associated building components by MR/PM number.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetBuildingComponentsByRequestNumberResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);



        app.MapPost(ApiEndpoints.Inspection.Create, async (CreateInspectionRequestCommand request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(request, token);
                var data = new BaseApiResponse<Guid>(result);

                return Results.Ok(data);
            })
            .WithName("CreateInspectionRequest")
            .WithTags(TagName)
            .WithDescription("Create new inspection request.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Guid>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.Inspection.Update, async (UpdateInspectionRequestCommand request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(request, token);
                var data = new BaseApiResponse<UpdateInspectionRequestResult>(result);

                return Results.Ok(data);
            })
            .WithName("UpdateInspectionRequest")
            .WithTags(TagName)
            .WithDescription("Update inspection request.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateInspectionRequestResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.Inspection.UpdateStatus, async (UpdateInspectionRequestStatusCommand request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(request, token);
            var data = new BaseApiResponse<UpdateInspectionRequestResult>(result);

            return Results.Ok(data);
        })
            .WithName("UpdateInspectionRequestStatus")
            .WithTags(TagName)
            .WithDescription("Update inspection request status.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateInspectionRequestResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);



        app.MapPost(ApiEndpoints.Inspection.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetInspectionRequestsQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryInspectionRequests")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.Inspection.UpdateBuildingComponent, async (UpdateBuildingComponentCommand request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(request, token);
                var data = new BaseApiResponse<UpdateBuildingComponentResult>(result);

                return Results.Ok(data);
            })
            .WithName("UpdateBuildingComponent")
            .WithTags(TagName)
            .WithDescription("Update building component.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateBuildingComponentResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapDelete(ApiEndpoints.Inspection.DeleteBuildingComponent, async ([FromBody] DeleteBuildingComponentCommand request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(request, token);
                var data = new BaseApiResponse<DeleteBuildingComponentResult>(result);

                return Results.Ok(data);
            })
            .WithName("DeleteBuildingComponent")
            .WithTags(TagName)
            .WithDescription("Delete building component.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<DeleteBuildingComponentResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.Inspection.UpdateProjectMonitoring, async (UpdateInspectionRequestProjectMonitoringCommand request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(request, token);
            var data = new BaseApiResponse<UpdateInspectionRequestResult>(result);

            return Results.Ok(data);
        })
            .WithName("UpdateInspectionRequestProjectMonitoring")
            .WithTags(TagName)
            .WithDescription("Create new inspection request with purpose Project Monitoring.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateInspectionRequestResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}