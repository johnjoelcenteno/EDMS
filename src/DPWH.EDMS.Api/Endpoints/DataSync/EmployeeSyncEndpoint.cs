using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.Users.Commands.SyncUser;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Models.DpwhResponses;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.DataSync;

public static class EmployeeSyncEndpoint
{
    public static IEndpointRouteBuilder MapEmployeeSync(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.DataSync.SyncEmployee, async ([FromRoute] string employeeId, IMediator mediator, IDpwhApiService dpwhApiService, CancellationToken token) =>
            {
                var response = await dpwhApiService.GetWithRetry<EmployeeResponse>(EmployeeResponse.Operation, employeeId);

                if (response?.Body?.Container?.Data is null)
                {
                    throw new AppException("Received invalid response");
                }

                var result = new BaseApiResponse<Employee>(response.Body.Container.Data);
                if (result.Data.EmployeeId is not null)
                {
                    //sync local copy
                    await mediator.Send(new SyncUserCommand(result.Data), token);
                }

                return result.Data.EmployeeId is not null ? Results.Ok(result) : Results.NotFound("Employee not found");
            })
            .WithName("SyncEmployeeById")
            .WithTags(DataSyncEndpointExtensions.Tag)
            .WithDescription("Retrieves a DPWH employee using employee Id from DPWH Api and then sync it in our local storage")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}