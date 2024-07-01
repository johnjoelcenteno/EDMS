using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Models.DpwhResponses;
using DPWH.EDMS.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.DpwhIntegrations;

public static class DpwhIntegrationsEndpoint
{
    private const string TagName = "DpwhIntegrations";

    public static IEndpointRouteBuilder MapDpwhIntegrations(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.DpwhIntegrations.Employee, async ([FromRoute] string employeeId, IDpwhApiService dpwhApiService) =>
            {
                var result = await dpwhApiService.GetWithRetry<EmployeeResponse>(EmployeeResponse.Operation, employeeId);
                var data = new BaseApiResponse<Employee>(result.Body.Container.Data);

                if (result?.Body?.Container?.Data is null)
                {
                    throw new AppException("Received invalid response");
                }

                return data.Data.EmployeeId is not null ? Results.Ok(data) : Results.NotFound("Employee not found");

            })
            .WithName("GetEmployeeById")
            .WithTags(TagName)
            .WithDescription("Retrieves a DPWH employee using employee Id from DPWH Api.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.DpwhIntegrations.EmployeeRaw, async ([FromRoute] string employeeId, IDpwhApiService dpwhApiService) =>
        {
            var response = await dpwhApiService.GetRaw(EmployeeResponse.Operation, employeeId);
            return Results.Ok(response);
        })
            .WithName("GetEmployeeByIdRaw")
            .WithTags(TagName)
            .WithDescription("Retrieves a DPWH employee using employee Id from DPWH Api.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.DpwhIntegrations.Position, async (IDpwhApiService dpwhApiService) =>
            {
                var response = await dpwhApiService.Get<PositionResponse>(PositionResponse.Operation);

                if (response?.Body?.Response?.Result?.Data is null)
                {
                    throw new AppException("Received invalid response");
                }

                var result = new BaseApiResponse<Position[]>(response.Body.Response.Result.Data);

                return Results.Ok(result);
            })
            .WithName("GetPositions")
            .WithTags(TagName)
            .WithDescription("Retrieves all position and salary grade info.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Position[]>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);


        app.MapGet(ApiEndpoints.DpwhIntegrations.GeoRegion, async (string? type, string? parentId, IDpwhApiService dpwhApiService) =>
        {
            var response = await dpwhApiService.GetLocationWithRetry<GeoRegionResponse>(GeoRegionResponse.Operation, type, parentId);

            if (response?.Body?.Response?.Result?.Data is null)
            {
                throw new AppException("Received invalid response");
            }

            var result = new BaseApiResponse<GeoRegion[]>(response.Body.Response.Result.Data);

            return Results.Ok(result);
        })
            .WithName("GetGeoRegions")
            .WithTags(TagName)
            .WithDescription("Retrieves a list of geo locations from DPWH Api by type (R, P, C, M, B) and ParentId")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GeoRegion[]>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.DpwhIntegrations.RequestingOffice, async (IDpwhApiService dpwhApiService) =>
        {
            var response = await dpwhApiService.GetWithRetry<RequestingOfficeResponse>(RequestingOfficeResponse.Operation);

            if (response?.Body?.Response?.Result?.Data is null)
            {
                throw new AppException("Received invalid response");
            }

            var result = new BaseApiResponse<ImplementingOffice[]>(response.Body.Response.Result.Data);

            return Results.Ok(result);
        })
            .WithName("GetRequestingOffices")
            .WithTags(TagName)
            .WithDescription("Retrieves a list of requesting offices from DPWH Api.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<ImplementingOffice[]>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
