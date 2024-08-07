using DPWH.EDMS.Application;
using DPWH.EDMS.Application.Features.RecordsManagement.Commands.CreateRecord;
using DPWH.EDMS.Application.Features.RecordsManagement.Commands.DeleteRecord;
using DPWH.EDMS.Application.Features.RecordsManagement.Queries;
using DPWH.EDMS.Application.Features.RecordsManagement.Queries.GetRecordById;
using DPWH.EDMS.Application.Features.RecordsManagement.Queries.GetRecordsByEmployeeIdQuery;
using DPWH.EDMS.Application.Features.RecordsManagement.Queries.GetRecordsQuery;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.RecordsManagement;

public static class RecordsEndpoint
{
    private const string TagName = "Records Management";

    public static IEndpointRouteBuilder MapRecords(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.RecordManagement.Get, async ([FromRoute] Guid id, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new GetRecordByIdQuery(id), token);

            var data = new BaseApiResponse<RecordModel>(result);

            return result is null ? Results.NotFound() : Results.Ok(data);

        })
            .WithName("GetRecordById")
            .WithTags(TagName)
            .WithDescription("Get record using id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<RecordModel>>()
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(ApiEndpoints.RecordManagement.Query, async (DataSourceRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetRecordsQuery(request));
                return result;
            })
            .WithName("QueryRecords")
            .WithTags(TagName)
            .WithDescription("Get records")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.RecordManagement.QueryByEmployeeId, async (DataSourceRequest request, string employeeId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetRecordsByEmployeeIdQuery(request, employeeId));
            return result;
        })
            .WithName("QueryRecordsByEmployeeId")
            .WithTags(TagName)
            .WithDescription("Get records by employeeId")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.RecordManagement.Create, async (CreateRecordModel model, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateRecordCommand(model.EmployeeId, model.RecordTypeId, model.RecordName, model.RecordUri));
                return result;
            })
            .WithName("CreateRecord")
            .WithTags(TagName)
            .WithDescription("Create new record")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<CreateResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);        

        app.MapDelete(ApiEndpoints.RecordManagement.Delete, async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteRecordRequests(id));
            var actionResult = new BaseApiResponse<Guid>(result);

            return Results.Ok(actionResult);
        })
        .WithName("DeleteRecord")
        .WithTags(TagName)
        .WithDescription("Delete record")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<DeleteResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
