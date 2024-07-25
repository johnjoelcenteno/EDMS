using DPWH.EDMS.Application.Features.Navigation.Commands.CreateMenuItem;
using DPWH.EDMS.Application.Features.Navigation.Queries;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Models.Navigation;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Navigation;

public static class NavigationEndpoints
{
    private const string TagName = "Navigation";

    public static IEndpointRouteBuilder MapNavigation(this IEndpointRouteBuilder builder)
    {
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

        return builder;
    }
}
