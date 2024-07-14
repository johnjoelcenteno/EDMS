using KendoNET.DynamicLinq;
using DPWH.EDMS.Application.Features.Reports.Queries;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Infrastructure.Context;

namespace DPWH.EDMS.Api.Endpoints.Reports;

public static class ReportsEndpoint
{
    private const string TagName = "Reports";

    public static IEndpointRouteBuilder MapReports(this IEndpointRouteBuilder app)
    {
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

        return app;
    }
}
