using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRates;
using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.UpdateRentalRates;
using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRatesById;
using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRates;
using DPWH.EDMS.Application.Models;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DPWH.EDMS.Api.Endpoints;

namespace DPWH.EDMS.Api.Endpoints.Inspections;

public static class RentalRatesEndpoint
{
    public const string TagName = "Rental Rates";

    public static IEndpointRouteBuilder MapRentalRates(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.RentalRates.Create, async (CreateRentalRatesCommand request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(request, token);
                var data = new BaseApiResponse<Guid>(result);

                return Results.Ok(data);
            })
            .WithName("CreateRentalRates")
            .WithTags(TagName)
            .WithDescription("Create new rental rates.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Guid>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.RentalRates.Update, async (UpdateRentalRatesCommand request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(request, token);
                var data = new BaseApiResponse<Guid>(result);

                return Results.Ok(data);
            })
            .WithName("UpdateRentalRates")
            .WithTags(TagName)
            .WithDescription("Update rental rates.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Guid>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.RentalRates.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetRentalRatesQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryRentalRates")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.RentalRates.Get, async ([FromRoute] Guid rentalRatePropertyId, IMediator mediator, CancellationToken token) =>
            {
                var request = new GetRentalRatesByIdQuery(rentalRatePropertyId);
                var response = await mediator.Send(request, token);

                var result = new BaseApiResponse<RentalRatesModel>(response);

                return result.Data == null ? Results.NotFound("Rental rate not found") : Results.Ok(result);
            })
            .WithName("GetRentalRatesById")
            .WithTags(TagName)
            .WithDescription("Get inspector inspection request by id.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<RentalRatesModel>>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
