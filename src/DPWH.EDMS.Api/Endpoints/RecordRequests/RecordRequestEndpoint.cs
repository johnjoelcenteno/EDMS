using DPWH.EDMS.Application.Features.RecordRequests.Commands.CreateRecordRequest;
using DPWH.EDMS.Application.Features.RecordRequests.Queries;
using DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestById;
using DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestsQuery;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Models.DpwhResponses;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.RecordRequests;

public static class RecordRequestEndpoint
{
    private const string TagName = "RecordRequests";

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

        app.MapPut(ApiEndpoints.RecordRequest.Update, async (string employeeId, IMediator mediator) =>
            {
                return "Ok";
            })
            .WithName("Update document request")
            .WithTags(TagName)
            .WithDescription("Update document requests")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapDelete(ApiEndpoints.RecordRequest.Delete, async (string employeeId, IMediator mediator) =>
        {
            return "Ok";
        })
        .WithName("Delete document request")
        .WithTags(TagName)
        .WithDescription("Delete document requests")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Employee>>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
