using DPWH.EDMS.Application.Features.Navigation.Commands.CreateMenuItem;
using DPWH.EDMS.Application.Features.Navigation.Commands.DeleteMenuItem;
using DPWH.EDMS.Application.Features.Navigation.Commands.UpdateMenuItem;
using DPWH.EDMS.Application.Features.Navigation.Queries;
using DPWH.EDMS.Application.Features.Navigation.Queries.GetMenuItemById;
using DPWH.EDMS.Application.Features.Navigation.Queries.GetMenuItemsByNavType;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Models.Navigation;
using DPWH.EDMS.Shared.Enums;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Navigation;

public static class NavigationEndpoints
{
    private const string TagName = "Navigation";

    public static IEndpointRouteBuilder MapNavigation(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(ApiEndpoints.Navigation.QueryByNavType, async (string navType, DataSourceRequest request, IMediator mediator, CancellationToken token) =>
         {
             var result = await mediator.Send(new QueryMenuItemsByNavTypeRequest(navType, request), token);
             return result;
         })
         .WithName("QueryMenuItemByNavType")
         .WithTags(TagName)
         .WithDescription("Queries menu item by NavType")
         .WithApiVersionSet(ApiVersioning.VersionSet)
         .HasApiVersion(1.0)
         .Produces<DataSourceResult>()
         .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
         .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapPost(ApiEndpoints.Navigation.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new QueryMenuItemsRequest(request), token);
            return result;
        })
        .WithName("QueryMenuItem")
        .WithTags(TagName)
        .WithDescription("Queries menu item")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<DataSourceResult>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapGet(ApiEndpoints.Navigation.GetById, async ([FromRoute] Guid id, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new GetMenuItemByIdQuery(id), token);

            var data = new BaseApiResponse<MenuItemModel>(result);

            return result is null ? Results.NotFound() : Results.Ok(data);

        })
            .WithName("GetMenuItem")
            .WithTags(TagName)
            .WithDescription("Get menu item using the id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<MenuItemModel>>()
            .Produces(StatusCodes.Status404NotFound);

        builder.MapPost(ApiEndpoints.Navigation.Create, async ([FromBody] CreateMenuItemModel model, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateMenuItemRequest(model));
            return new BaseApiResponse<Guid>(result);
        })
       .WithName("CreateMenuItem")
       .WithTags(TagName)
       .WithDescription("Creates new menu item")
       .WithApiVersionSet(ApiVersioning.VersionSet)
       .HasApiVersion(1.0)
       .Produces<CreateResponse>()
       .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
       .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapPut(ApiEndpoints.Navigation.Update, async ([FromRoute] Guid Id, UpdateMenuItemModel model, IMediator mediator) =>
        {
            var result = await mediator.Send(new UpdateMenuItemRequest(Id, model));
            var data = new BaseApiResponse<Guid?>(result);
            return result is null ? Results.NotFound() : Results.Ok(data);
        })
        .WithName("UpdateMenuItem")
        .WithTags(TagName)
        .WithDescription("Updates menu item")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Guid?>>()
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        builder.MapDelete(ApiEndpoints.Navigation.Delete, async (Guid Id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteMenuItemCommand(Id));
            return new DeleteResponse(result);
        })
        .WithName("DeleteMenuItem")
        .WithTags(TagName)
        .WithDescription("Deletes menu item")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<DeleteResponse>(StatusCodes.Status200OK)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return builder;
    }
}
