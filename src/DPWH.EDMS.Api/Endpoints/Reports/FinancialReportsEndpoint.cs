using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsByPolicyNo;
using DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsByRegionAndYear;
using DPWH.EDMS.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Reports;

public static class FinancialReportsEndpoint
{
    private const string TagName = "FinancialReports";

    public static IEndpointRouteBuilder MapFinancialReports(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.FinancialReports.InsurancePolicy, async (string policyNo, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAssetsByPolicyNoQuery(policyNo), token);
                var data = new BaseApiResponse<GetAssetsByPolicyNoResult>(result);

                return Results.Ok(data);
            })
            .WithName("GetAssetsByPolicyNo")
            .WithTags(TagName)
            .WithDescription("Retrieves assets by `financial detail policy number`.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetAssetsByPolicyNoResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.FinancialReports.InsuranceSummary, async (string regionId, int year, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAssetsByRegionAndYearQuery(regionId, year), token);
                var data = new BaseApiResponse<GetAssetsByRegionAndYearResult>(result);

                return Results.Ok(data);
            })
            .WithName("GetAssetsByRegionAndYearResult")
            .WithTags(TagName)
            .WithDescription("Retrieves assets by region and effectivity year.")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetAssetsByRegionAndYearResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }
}