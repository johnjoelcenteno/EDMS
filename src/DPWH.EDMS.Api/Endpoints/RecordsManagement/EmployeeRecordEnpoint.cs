using DPWH.EDMS.Application;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Models.DpwhResponses;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.RecordsManagement;

public static class EmployeeRecordEnpoint
{
    private const string TagName = "EmployeeRecords";

    public static IEndpointRouteBuilder MapEmployeeRecordEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.EmployeeRecordEndpoints.Query, async ([FromBody] DataSourceRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(new QueryEmployeeRequest(request));
                return result;
            })
            .WithName("Query employee request")
            .WithTags(TagName)
            .WithDescription("Get all employee records")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.EmployeeRecordEndpoints.QueryById, async (Guid employeeRecordId, IDpwhApiService dpwhApiService) =>
           {

               return "Ok";
           })
           .WithName("Query employee request by id")
           .WithTags(TagName)
           .WithDescription("Get employee by employee id")
           .WithApiVersionSet(ApiVersioning.VersionSet)
           .HasApiVersion(1.0)
           .Produces<BaseApiResponse<Employee>>()
           .Produces(StatusCodes.Status404NotFound)
           .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.EmployeeRecordEndpoints.Create, async (CreateUpdateEmployeeModel model, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateEmployeeRequest(model));
                return result;
            })
            .WithName("Create Employee")
            .WithTags(TagName)
            .WithDescription("Create employee")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.EmployeeRecordEndpoints.Update, async ([FromRoute] Guid Id, CreateUpdateEmployeeModel model, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdateEmployeeRequest(Id, model));
                return result;
            })
            .WithName("Update employee")
            .WithTags(TagName)
            .WithDescription("Get all document requests")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapDelete(ApiEndpoints.EmployeeRecordEndpoints.Delete, async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteEmployeeRequest(id));
            return result;
        })
        .WithName("Delete Employee")
        .WithTags(TagName)
        .WithDescription("Delete employee")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Employee>>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
