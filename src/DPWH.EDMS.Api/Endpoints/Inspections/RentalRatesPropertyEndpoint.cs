using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesProperty;
using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.UpdateRentalRatesProperty;
using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesProperty;
using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesPropertyById;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Inspections;

public static class RentalRatesPropertyEndpoint
{
    public static IEndpointRouteBuilder MapRentalRatesProperty(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.RentalRates.Property.Create, async (CreateRentalRatesPropertyCommand request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(request, token);
            var data = new BaseApiResponse<Guid>(result);

            return Results.Ok(data);
        })
            .WithName("CreateRentalRateProperty")
            .WithTags(RentalRatesEndpoint.TagName)
            .WithDescription("Create new rental rate property.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Guid>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.RentalRates.Property.Update, async (UpdateRentalRatesPropertyCommand request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(request, token);
            var data = new BaseApiResponse<Guid>(result);

            return Results.Ok(data);
        })
            .WithName("UpdateRentalRateProperty")
            .WithTags(RentalRatesEndpoint.TagName)
            .WithDescription("Update rental rate property.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Guid>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.RentalRates.Property.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new GetRentalRatesPropertyQuery(request), token);

            return Results.Ok(result);
        })
            .WithName("QueryRentalRateProperty")
            .WithTags(RentalRatesEndpoint.TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.RentalRates.Property.Get, async ([FromRoute] Guid id, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetRentalRatesPropertyByIdQuery(id);
            var response = await mediator.Send(request, token);

            var result = new BaseApiResponse<RentalRatesPropertyModel>(response);

            return result.Data == null ? Results.NotFound("Rental rate property not found") : Results.Ok(result);
        })
            .WithName("GetRentalRatePropertyById")
            .WithTags(RentalRatesEndpoint.TagName)
            .WithDescription("Get rental rate property by id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<RentalRatesPropertyModel>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
