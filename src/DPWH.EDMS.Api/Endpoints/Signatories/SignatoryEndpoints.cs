using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api;

public static class SignatoryEndpoints
{
    private const string TagName = "Signatories";
    public static IEndpointRouteBuilder MapSignatoryEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(ApiEndpoints.Signatories.Create, async ([FromBody] CreateSignatoryModel model, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateSignatoryRequest(model));
            return new BaseApiResponse<Guid>(result);
        })
        .WithName("Creates new signatory")
        .WithTags(TagName)
        .WithDescription("Creates new signatory")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Guid>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapPost(ApiEndpoints.Signatories.Query, async (DataSourceRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new QuerySignatoryRequest(request));
            return Results.Ok(result);
        })
        .WithName("Query signatories")
        .WithTags(TagName)
        .WithDescription("Queries signatories")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<DataSourceResult>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapPut(ApiEndpoints.Signatories.Update, async ([FromRoute] Guid Id, [FromBody] UpdateSignatoryModel model, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateSignatoryRequest(Id, model));
            var data = new BaseApiResponse<Guid?>(result);
            return data is null ? Results.NotFound() : Results.Ok(data);
        })
        .WithName("Updates signatory")
        .WithTags(TagName)
        .WithDescription("Updates signatory")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Guid?>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapDelete(ApiEndpoints.Signatories.Delete, async ([FromRoute] Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteSignatoryRequest(id));
            var data = new BaseApiResponse<Guid?>(result);
            return data is null ? Results.NotFound() : Results.Ok(data);
        })
        .WithName("Delete signatory")
        .WithTags(TagName)
        .WithDescription("Deletes signatories")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Guid?>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return builder;
    }
}
