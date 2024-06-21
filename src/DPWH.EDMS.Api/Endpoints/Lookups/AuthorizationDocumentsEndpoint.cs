using DPWH.EDMS.Application.Features.Lookups.Queries.GetAuthorizationDocuments;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Lookups;

public static class AuthorizationDocumentsEndpoint
{
    public const string AuthorizationDocumentCacheTag = "AuthorizationDocumentCacheTag";
    public static IEndpointRouteBuilder MapAuthorizationDocuments(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Lookups.AuthorizationDocuments, async (IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAuthorizationDocumentsQuery(), token);

                var data = new BaseApiResponse<IEnumerable<GetAuthorizationDocumentsResult>>(result);

                return Results.Ok(data);
            })
            .WithName("GetAuthorizationDocuments")
            .WithTags(CommonLookupsEndpointExtensions.Tag)
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<IEnumerable<GetAuthorizationDocumentsResult>>>()
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .CacheOutput(builder => builder.Tag(AuthorizationDocumentCacheTag));

        return app;
    }
}