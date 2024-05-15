using DPWH.EDMS.Application.Features.Inspections.Queries.RentalRates.GetRentalRates;
using MediatR;
using KendoNET.DynamicLinq;
using DPWH.EDMS.Application.Features.Reports.Queries;
using DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriInd;
using DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriRSum;
using DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriVal1;
using DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriVal2;
using DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsPerBuildingStatus;
using DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsPerPropertyCondition;
using DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsPerRegion;
using DPWH.EDMS.Application.Features.Reports.Queries.GetInventoriesByArea;
using DPWH.EDMS.Application.Features.Reports.Queries.GetInventoriesByFinancialDetail;
using DPWH.EDMS.Application.Features.Reports.Queries.GetInventoriesByFundingHistory;
using DPWH.EDMS.Application.Features.Reports.Queries.GetInventoriesByLocation;
using DPWH.EDMS.Application.Features.Reports.Queries.GetInventoriesByPropertyDetail;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Infrastructure.Context;
using DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriDSum;
using DPWH.EDMS.Application.Features.Reports.Queries.DpwhReports.PriBomEval;

namespace DPWH.EDMS.Api.Endpoints.Reports;

public static class ReportsEndpoint
{
    private const string TagName = "Reports";

    public static IEndpointRouteBuilder MapReports(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Reports.AssetsPerRegion, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAssetsPerRegionQuery(request), token);

                return result;
            })
            .WithName("AssetsPerRegion")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.AssetsPerPropertyCondition, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAssetsPerPropertyConditionQuery(request), token);

                return result;
            })
            .WithName("AssetsPerPropertyCondition")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Graph")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.AssetsPerBuildingStatus, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetAssetsPerBuildingStatusQuery(request), token);

                return result;
            })
            .WithName("AssetsPerBuildingStatus")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Graph")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.UsersPerRole, (DataSourceRequest request, AppIdpDataContext idpContext) =>
            {
                var rolesPerUser = ApplicationRoles.UserAccessMapping
                    .Select(r => new AssestPerConditionModel
                    {
                        NameOfItem = r.Value,
                        NumberOfItems = idpContext.UserClaims.Count(uc => uc.ClaimValue == r.Key)
                    })
                    .AsQueryable();

                return Results.Ok(rolesPerUser.ToDataSourceResult(request));
            })
            .WithName("UsersPerRole")
            .WithTags(TagName)
            .WithDescription("Returns a list of roles and the number of users assigned to it")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.Users, (
            DataSourceRequest request, AppIdpDataContext idpContext, CancellationToken token) =>
        {
            //TODO: Fix this query, should not call `ToList()`
            var usersWithRoles = (from user in idpContext.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      user.Email,
                                      FirstName = user.UserBasicInfo == null ? null : user.UserBasicInfo.FirstName,
                                      LastName = user.UserBasicInfo == null ? null : user.UserBasicInfo.LastName,
                                      MobileNumber = user.PhoneNumber,
                                      Position = user.EmployeeInfo == null ? null : user.EmployeeInfo.Position,
                                      Office = user.EmployeeInfo == null ? null : user.EmployeeInfo.DistrictEngineeringOffice,
                                      SubOffice = user.EmployeeInfo == null ? null : user.EmployeeInfo.RegionalOfficeRegion,
                                      user.Created,
                                      RoleNames = idpContext.UserClaims
                                          .Where(u => u.UserId == user.Id && (u.ClaimType == "role" || u.ClaimType == "account_status"))
                                          .Select(uc => uc.ClaimValue)
                                          .ToList()
                                  }).ToList().Select(p => new UserReportModel
                                  {
                                      Id = p.UserId,
                                      UserName = p.Username,
                                      Position = p.Position,
                                      Email = p.Email,
                                      Office = p.Office,
                                      SubOffice = p.SubOffice,
                                      Role = string.Join(',', p.RoleNames),
                                      UserAccess = ApplicationRoles.GetDisplayRoleName(p.RoleNames.FirstOrDefault(), "None"),
                                      TimeIn = DateTime.Now,
                                      TimeOut = DateTime.Now,
                                      CreatedDate = p.Created,
                                      FirstName = p.FirstName,
                                      LastName = p.LastName,
                                      Location = "Location Static Text",
                                      Gender = "Male Static Text"
                                  }).AsQueryable();

            return Results.Ok(usersWithRoles.ToDataSourceResult(request));

        })
            .WithName("Users")
            .WithTags(TagName)
            .WithDescription("Queries application user")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));


        app.MapPost(ApiEndpoints.Reports.QueryInventory, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var filter = request.Filter.Value;
                object result = null;

                result = filter switch
                {
                    "FinancialDetails" => await mediator.Send(new GetInventoriesByFinancialDetailQuery(request), token),
                    "Area" => await mediator.Send(new GetInventoriesByAreaQuery(request), token),
                    "Location" => await mediator.Send(new GetInventoriesByLocationQuery(request), token),
                    "FundingHistory" => await mediator.Send(new GetInventoriesByFundingHistoryQuery(request), token),
                    "PropertyDetails" => await mediator.Send(new GetInventoriesByPropertyDetailQuery(request), token),
                    _ => TypedResults.NotFound(),// Handle the case when the type is not recognized
                };
                return result;
            })
            .WithName("QueryInventoryReport")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryPropertyDetails, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetInventoriesByPropertyDetailQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryPropertyDetails")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryFinancialDetails, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetInventoriesByFinancialDetailQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryFinancialDetails")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryArea, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetInventoriesByAreaQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryArea")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryLocation, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetInventoriesByLocationQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryLocation")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryFundingHistory, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetInventoriesByFundingHistoryQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryFundingHistory")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        //Rental Rates
        app.MapPost(ApiEndpoints.Reports.QueryRentalRates, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetRentalRatesQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryRentalRatesReport")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryPRIDSum, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetPriDSumQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryPRIDSum")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryPRIRSum, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetPriRSumQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryPRIRSum")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryPRIBomEval, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetPribomEvalQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryPRIBOMEval")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryPRIVal1, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetPriVal1Query(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryPRIVal1")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryPRIVal2, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetPriVal2Query(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryPRIVal2")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        app.MapPost(ApiEndpoints.Reports.QueryPRIInd, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var result = await mediator.Send(new GetPriIndQuery(request), token);

                return Results.Ok(result);
            })
            .WithName("QueryPRIInd")
            .WithTags(TagName)
            .WithDescription("Query via datasource. Ideal when you need to access resource via Grid")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        return app;
    }
}
