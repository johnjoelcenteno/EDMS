﻿using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application;
using DPWH.EDMS.Application.Features.RecordTypes.Commands;
using DPWH.EDMS.Application.Features.RecordTypes.Commands.DeleteRecordType;
using DPWH.EDMS.Application.Features.RecordTypes.Queries;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
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

        builder.MapGet(ApiEndpoints.RecordTypes.QueryById, async ([FromRoute] Guid Id, IMediator mediator) =>
        {
            var result = await mediator.Send(new QueryRecordTypeByIdRequest(Id));
            var data = new BaseApiResponse<QueryRecordTypesModel?>(result);
            return data is null ? Results.NotFound() : Results.Ok(data);
        })
        .WithName("Query record types by id")
        .WithTags(TagName)
        .WithDescription("Queries record type by id")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<QueryRecordTypesModel?>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapGet(ApiEndpoints.RecordTypes.QueryByCategory, async ([FromRoute] string category, IMediator mediator) =>
       {
           var result = await mediator.Send(new QueryRecordTypesByCategoryRequest(category));
           var data = new BaseApiResponse<List<QueryRecordTypesModel>>(result);
           return data is null ? Results.NotFound() : Results.Ok(data);
       })
       .WithName("Query record types by category")
       .WithTags(TagName)
       .WithDescription("Query record types by category")
       .WithApiVersionSet(ApiVersioning.VersionSet)
       .HasApiVersion(1.0)
       .Produces<BaseApiResponse<List<QueryRecordTypesModel>>>()
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
        .Produces<DataSourceResult>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapPut(ApiEndpoints.RecordTypes.Update, async ([FromRoute] Guid Id, UpdateRecordTypeModel model, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateRecordTypeRequest(Id, model));
            var data = new BaseApiResponse<Guid?>(result);
            return result is null ? Results.NotFound() : Results.Ok(data);
        })
        .WithName("UpdateRecordType")
        .WithTags(TagName)
        .WithDescription("Updates record type")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Guid?>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapDelete(ApiEndpoints.RecordTypes.Delete, async (Guid Id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteRecordTypeCommand(Id));            
            return new DeleteResponse(result);
        })
        .WithName("DeleteRecordType")
        .WithTags(TagName)
        .WithDescription("Deletes record type")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<DeleteResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return builder;
    }
}