using DPWH.EDMS.Application.Features.Addresses.Commands.CreateAddress;
using DPWH.EDMS.Application.Features.Addresses.Commands.DeleteAddress;
using DPWH.EDMS.Application.Features.Addresses.Commands.UpdateAddress;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Addresses;

public static class AddressesEndpoint
{
    private const string TagName = "Address";

    public static IEndpointRouteBuilder MapAddresses(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Addresses.CreateAddress, async (CreateAddressCommand request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(request, token);
                var data = new BaseApiResponse<CreateAddressResult>(result);

                return Results.Ok(data);
            })
            .WithName("CreateAddress")
            .WithTags(TagName)
            .WithDescription("Creates new address")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<CreateAddressResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.Addresses.UpdateAddress, async (UpdateAddressCommand request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(request, token);
                var data = new BaseApiResponse<UpdateAddressResult>(result);

                return Results.Ok(data);
            })
            .WithName("UpdateAddress")
            .WithTags(TagName)
            .WithDescription("Updates address")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateAddressResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapDelete(ApiEndpoints.Addresses.DeleteAddress, async (string id, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new DeleteAddressCommand(id), token);
                var data = new BaseApiResponse<DeleteAddressResult>(result);

                return Results.Ok(data);
            })
            .WithName("DeleteAddress")
            .WithTags(TagName)
            .WithDescription("Delete address")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<DeleteAddressResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
