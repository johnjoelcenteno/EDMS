using DPWH.EDMS.Application.Features.RecordRequests.Commands.CreateRecordRequest;
using DPWH.EDMS.Application.Features.RecordRequests.Commands.UpdateRecordRequestStatus;
using DPWH.EDMS.Application.Features.RecordRequests.Queries;
using DPWH.EDMS.Application.Features.RecordRequests.Queries.GetCountRecordsByStatusQuery;
using DPWH.EDMS.Application.Features.RecordRequests.Queries.GetMonthlyRequests;
using DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestById;
using DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestsByEmployeeIdQuery;
using DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestsByStatusQuery;
using DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestsQuery;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Models.DpwhResponses;
using DPWH.EDMS.Application.Models.RecordRequests;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.RecordRequests;

public static class RecordRequestEndpoint
{
    private const string TagName = "Requests Management";

    public static IEndpointRouteBuilder MapRecordRequests(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.RecordRequest.Get, async ([FromRoute] Guid id, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new GetRecordRequestByIdQuery(id), token);

            var data = new BaseApiResponse<RecordRequestModel>(result);

            return result is null ? Results.NotFound() : Results.Ok(data);

        })
            .WithName("GetRecordRequest")
            .WithTags(TagName)
            .WithDescription("Get record request using the id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<RecordRequestModel>>()
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(ApiEndpoints.RecordRequest.Query, async (DataSourceRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetRecordRequestsQuery(request));
                return result;
            })
            .WithName("QueryRecordRequests")
            .WithTags(TagName)
            .WithDescription("Get record request")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        app.MapGet(ApiEndpoints.RecordRequest.Count, async (string status,IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCountRecordsByStatusQuery(status));
            var data = new BaseApiResponse<RecordRequestStatusCountModel>(result);

            return Results.Ok(data);
        })
            .WithName("CountRecordRequestsByStatus")
            .WithTags(TagName)
            .WithDescription("Count the Record Request base on Status")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<RecordRequestStatusCountModel>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.RecordRequest.QueryByEmployeeId, async (DataSourceRequest request, string employeeId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetRecordRequestsByEmployeeIdQuery(request, employeeId));
            return result;
        })
            .WithName("QueryRecordRequestsByEmployeeId")
            .WithTags(TagName)
            .WithDescription("Get record request by employeeId")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.RecordRequest.GetMonthlyRequests, async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetMonthlyRequestQuery());

            var data = new BaseApiResponse<IEnumerable<GetMonthlyRequestModel>>(result);

            return result is null ? Results.NotFound() : Results.Ok(data);
        })
            .WithName("GetMonthlyRequestsTotalCount")
            .WithTags(TagName)
            .WithDescription("Get monthly requests count")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetMonthlyRequestModel>>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.RecordRequest.QueryByStatus, async (DataSourceRequest request, string status, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetRecordRequestsByStatusQuery(request, status));
            return result;
        })
            .WithName("QueryRecordRequestsByStatus")
            .WithTags(TagName)
            .WithDescription("Get record request by status")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.RecordRequest.Create, async (CreateRecordRequest model, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateRecordRequestCommand(model));
                return result;
            })
            .WithName("CreateRecordRequest")
            .WithTags(TagName)
            .WithDescription("Create new record request")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<CreateResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.RecordRequest.UpdateStatus, async (UpdateRecordRequestStatus model, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new UpdateRecordRequestStatusCommand(model), cancellationToken);
                return result;
            })
            .WithName("UpdateRecordRequestStatus")
            .WithTags(TagName)
            .WithDescription("Update record request status")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateResponse>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapDelete(ApiEndpoints.RecordRequest.Delete, async (Guid id, IMediator mediator) =>
        {
            return "Ok";
        })
        .WithName("DeleteRecordRequest")
        .WithTags(TagName)
        .WithDescription("Delete record requests")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<DeleteResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
