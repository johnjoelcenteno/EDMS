using DPWH.EDMS.Application.Features.Lookups.Queries;
using DPWH.EDMS.Application.Features.Lookups.Queries.GetEmployeeDocuments;
using DPWH.EDMS.Application.Features.Lookups.Queries.GetEmployeeRecords;
using DPWH.EDMS.Application.Features.Lookups.Queries.GetIssuances;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class RecordTypesEndpoint
{    
    public const string IssuanceCacheTag = "IssuanceCacheTag";
    public const string EmployeeRecordCacheTag = "EmployeeRecordCacheTag";
    public const string EmployeeDocumentCacheTag = "EmployeeDocumentCacheTag";


    public static IEndpointRouteBuilder MapRecordTypes(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.Issuances, async (IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new GetIssuancesQuery(), token);

            var data = new BaseApiResponse<IEnumerable<GetLookupResult>>(result);

            return Results.Ok(data);
        })
            .WithName("GetIssuances")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetLookupResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(IssuanceCacheTag));

        app.MapGet(ApiEndpoints.Lookups.EmployeeRecords, async (IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new GetEmployeeRecordsQuery(), token);

            var data = new BaseApiResponse<IEnumerable<GetLookupResult>>(result);

            return Results.Ok(data);
        })
            .WithName("GetEmployeeRecords")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetLookupResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(EmployeeRecordCacheTag));

        app.MapGet(ApiEndpoints.Lookups.EmployeeDocuments, async (IMediator mediator, CancellationToken token) =>
        {
            var result = await mediator.Send(new GetEmployeeDocumentsQuery(), token);

            var data = new BaseApiResponse<IEnumerable<GetLookupResult>>(result);

            return Results.Ok(data);
        })
            .WithName("GetEmployeeDocuments")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetLookupResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(EmployeeDocumentCacheTag));

        return app;
    }
}