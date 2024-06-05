using DPWH.EDMS.Application;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Models.DpwhResponses;
using DPWH.EDMS.Domain.Exceptions;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.DpwhIntegrations;

public static class DocumentRequestEndpoint
{
    private const string TagName = "DocumentRequests";

    public static IEndpointRouteBuilder MapDocumentRequestEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.DocumentRequestEndpoint.Query, async (DataSourceRequest request, IMediator mediator) =>
            {
                var result = await mediator.Send(new DocumentQueryRequest(request));
                return result;
            })
            .WithName("Query document request")
            .WithTags(TagName)
            .WithDescription("Get document request")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.DocumentRequestEndpoint.Create, async (CreateUpdateDocumentRequestModel model, Guid employeeRecordId, Guid documentRecordsId, IMediator mediator) =>
            {
                var result = await mediator.Send(new CreateDocumentRequest(employeeRecordId, documentRecordsId, model));
                return result;
            })
            .WithName("Create DocumentRequest")
            .WithTags(TagName)
            .WithDescription("Create document request")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.DocumentRequestEndpoint.Update, async (string employeeId, IMediator mediator) =>
            {
                return "Ok";
            })
            .WithName("Update document request")
            .WithTags(TagName)
            .WithDescription("Update document requests")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapDelete(ApiEndpoints.DocumentRequestEndpoint.Delete, async (string employeeId, IMediator mediator) =>
        {
            return "Ok";
        })
        .WithName("Delete document request")
        .WithTags(TagName)
        .WithDescription("Delete document requests")
        .WithApiVersionSet(ApiVersioning.VersionSet)
        .HasApiVersion(1.0)
        .Produces<BaseApiResponse<Employee>>()
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
