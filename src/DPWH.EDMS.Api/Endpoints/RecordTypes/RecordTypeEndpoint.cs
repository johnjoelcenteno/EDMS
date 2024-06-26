using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application;
using DPWH.EDMS.Application.Features.RecordRequests.Commands.CreateRecordRequest;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api;

public static class RecordTypeEndpoints
{
    private const string TagName = "RecordTypes";

    public static IEndpointRouteBuilder MapRecordTypes(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(ApiEndpoints.RecordTypes.Create, async ([FromBody] CreateRecordTypeModel model, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateRecordTypeRequest(model));
            return new BaseApiResponse<Guid>(result);
        })
        .WithName("Create record type")
        .WithTags(TagName)
        .WithDescription("Creates new record type")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Guid>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapPost(ApiEndpoints.RecordTypes.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new QueryRecordTypesRequest(request), token);
            return result;
        })
        .WithName("Query record type")
        .WithTags(TagName)
        .WithDescription("Queries record type")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<DataSourceResult>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapPut(ApiEndpoints.RecordTypes.Update, async ([FromRoute] Guid Id, UpdateRecordTypeModel model, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateRecordTypeRequest(Id, model));
            var data = new BaseApiResponse<Guid?>(result);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("Update record type")
        .WithTags(TagName)
        .WithDescription("Updates record type")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Guid?>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapDelete(ApiEndpoints.RecordTypes.Delete, async (Guid Id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteRecordTypeRequest(Id));
            var data = new BaseApiResponse<Guid?>(result);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("Delete record type")
        .WithTags(TagName)
        .WithDescription("Deletes record type")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Guid?>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return builder;
    }
}