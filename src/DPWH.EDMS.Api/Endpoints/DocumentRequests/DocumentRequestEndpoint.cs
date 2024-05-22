using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.Application.Models.DpwhResponses;
using DPWH.EDMS.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.DpwhIntegrations;

public static class DocumentRequestEndpoint
{
    private const string TagName = "DocumentRequests";

    public static IEndpointRouteBuilder MapDocumentRequestEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.DocumentRequestEndpoint.Query, async (string employeeId, IDpwhApiService dpwhApiService) =>
            {
                return "Ok";
            })
            .WithName("Query document request")
            .WithTags(TagName)
            .WithDescription("Get all document requests")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<Employee>>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}
