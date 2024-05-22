using DPWH.EDMS.Api.Endpoints.DpwhIntegrations;

namespace DPWH.EDMS.Api;

public static class DocumentRequestEndpointExtensions
{
    public static IEndpointRouteBuilder MapDocumentRequestsEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapDocumentRequestEndpoint();
        return builder;
    }
}
