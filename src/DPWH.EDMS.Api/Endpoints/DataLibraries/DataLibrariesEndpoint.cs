using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Features.DataLibrary.Commands.AddDataLibrary;
using DPWH.EDMS.Application.Features.DataLibrary.Commands.CascadePropertyUpdate;
using DPWH.EDMS.Application.Features.DataLibrary.Commands.DeleteDataLibrary;
using DPWH.EDMS.Application.Features.DataLibrary.Commands.RecoverDataLibrary;
using DPWH.EDMS.Application.Features.DataLibrary.Commands.UpdateDataLibrary;
using DPWH.EDMS.Application.Features.DataLibrary.Queries.GetDataLibrary;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace DPWH.EDMS.Api.Endpoints.DataLibraries;

public static class DataLibrariesEndpoint
{
    private const string TagName = "DataLibraries";

    public static IEndpointRouteBuilder MapDataLibraries(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.DataLibraries.GetAll, async (DataSourceRequest request,IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetDataLibraryQuery(request), token);

                return result;
            })
            .WithName("GetDataLibraries")
            .WithTags(TagName)
            .WithDescription("Get all data libraries")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(TagName));

        app.MapPost(ApiEndpoints.DataLibraries.Add, async (AddDataLibraryCommand request, IMediator mediator,
            IOutputCacheStore cache, CancellationToken token) =>
            {
                var result = await mediator.Send(request, token);
                var data = new BaseApiResponse<AddDataLibraryResult>(result);

                await cache.EvictByTagAsync(TagName, token);

                return Results.Ok(data);
            })
            .WithName("AddDataLibrary")
            .WithTags(TagName)
            .WithDescription("Add data library")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<AddDataLibraryResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.DataLibraries.Update, async (UpdateDataLibraryCommand request, IMediator mediator,
            IOutputCacheStore cache, CancellationToken token) =>
            {
                var result = await mediator.Send(request, token);
                var data = new BaseApiResponse<UpdateDataLibraryResult>(result);

                await mediator.Send(new CascadePropertyUpdateCommand(result), token);
                await cache.EvictByTagAsync(TagName, token);

                return Results.Ok(data);
            })
            .WithName("UpdateDataLibrary")
            .WithTags(TagName)
            .WithDescription("Updates a data library value")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateDataLibraryResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapDelete(ApiEndpoints.DataLibraries.Delete, async ([FromRoute] Guid id, IMediator mediator,
            IOutputCacheStore cache, CancellationToken token) =>
            {
                var result = await mediator.Send(new DeleteDataLibraryCommand(id), token);
                var data = new BaseApiResponse<DeleteDataLibraryResult>(result);

                await cache.EvictByTagAsync(TagName, token);

                return Results.Ok(data);
            })
            .WithName("DeleteDataLibrary")
            .WithTags(TagName)
            .WithDescription("Soft deletes a data library")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<DeleteDataLibraryResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.DataLibraries.Recover, async ([FromRoute] Guid id, IMediator mediator,
            IOutputCacheStore cache, CancellationToken token) =>
            {
                var result = await mediator.Send(new RecoverDataLibraryCommand(id), token);
                var data = new BaseApiResponse<RecoverDataLibraryResult>(result);

                await cache.EvictByTagAsync(TagName, token);

                return Results.Ok(data);
            })
            .WithName("RecoverDataLibrary")
            .WithTags(TagName)
            .WithDescription("Recovers a soft deleted data library")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<RecoverDataLibraryResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}