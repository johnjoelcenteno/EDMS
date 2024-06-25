using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api;

public static class RecordTypeMappingEndpoint
{
    private const string TagName = "RecordTypeMappings";

    public static IEndpointRouteBuilder MapRecordTypeMappings(this IEndpointRouteBuilder builder) // typo here change it later
    {
        builder.MapPost(ApiEndpoints.RecordTypeMapping.Create, (CreateRecordTypeMappingModel model, IMediator mediator) =>
        {

        })
        .WithName("Create record type mapping")
        .WithTags(TagName)
        .WithDescription("Creates new record type mapping")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        // .Produces<BaseApiResponse<CreateAddressResult>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapGet(ApiEndpoints.RecordTypeMapping.Query, (IMediator mediator) =>
        {

        })
        .WithName("Query record type mappings")
        .WithTags(TagName)
        .WithDescription("Queries record type mappings")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        // .Produces<BaseApiResponse<CreateAddressResult>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapPut(ApiEndpoints.RecordTypeMapping.Update, ([FromRoute] Guid Id, UpdateRecordTypeMappingModel model, IMediator mediator) =>
        {

        })
        .WithName("Update record type mapping")
        .WithTags(TagName)
        .WithDescription("Updates record type mapping")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        // .Produces<BaseApiResponse<CreateAddressResult>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapDelete(ApiEndpoints.RecordTypeMapping.Delete, (IMediator mediator) =>
        {

        })
        .WithName("Delete record type mapping")
        .WithTags(TagName)
        .WithDescription("Deletes record type mapping")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        // .Produces<BaseApiResponse<CreateAddressResult>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return builder;
    }
}
