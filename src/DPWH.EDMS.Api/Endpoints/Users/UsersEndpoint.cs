using DPWH.EDMS.Api.Endpoints;
using DPWH.EDMS.Application.Contracts.Services;
using DPWH.EDMS.Application.Features.AuditLogs.Commands.CreateModifyAccessLog;
using DPWH.EDMS.Application.Features.RecordRequests.Commands.SaveUploadedFile;
using DPWH.EDMS.Application.Features.Signatories.Commands;
using DPWH.EDMS.Application.Features.Signatories.Queries;
using DPWH.EDMS.Application.Features.Users.Commands.CreateUser;
using DPWH.EDMS.Application.Features.Users.Commands.CreateUserWithRole;
using DPWH.EDMS.Application.Features.Users.Commands.DeactivateUser;
using DPWH.EDMS.Application.Features.Users.Commands.RemoveUser;
using DPWH.EDMS.Application.Features.Users.Commands.UpdateUser;
using DPWH.EDMS.Application.Features.Users.Commands.UploadSignature;
using DPWH.EDMS.Application.Features.Users.Queries.GetUserById;
using DPWH.EDMS.Application.Features.Users.Queries.GetUsers;
using DPWH.EDMS.Application.Models;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.Infrastructure.Storage;
using KendoNET.DynamicLinq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Api.Endpoints.Users;

public static class UsersEndpoint
{
    private const string TagName = "Users";

    public static IEndpointRouteBuilder MapUsers(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Users.Get, async ([FromRoute] Guid id, IMediator mediator, CancellationToken token) =>
            {
                var request = new GetUserByIdQuery(id);
                var response = await mediator.Send(request, token);
                var result = new BaseApiResponse<GetUserByIdResult>(response);

                return Results.Ok(result);
            })
            .WithName("GetUserById")
            .WithTags(TagName)
            .WithDescription("Get user by id")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetUserByIdResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.Users.GetByEmployeeId, async (string employeeId, IMediator mediator, CancellationToken token) =>
        {
            var request = new GetUserByEmployeeId(employeeId);
            var response = await mediator.Send(request, token);
            var result = new BaseApiResponse<GetUserByIdResult>(response);

            return Results.Ok(result);
        })
            .WithName("GetUserByEmployeeId")
            .WithTags(TagName)
            .WithDescription("Get user by employee id")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<GetUserByIdResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.Users.Create, async (CreateUserCommand request, IMediator mediator, CancellationToken token) =>
            {
                var createUserResult = await mediator.Send(request, token);

                var userManagementLog = new CreateModifyAccessLogCommand
                {
                    UserId = createUserResult.Id,
                    Action = "Add"
                };
                await mediator.Send(userManagementLog, token);

                var result = new BaseApiResponse<CreateUserResult>(createUserResult);

                return Results.Ok(result);
            })
            .WithName("CreateUser")
            .WithTags(TagName)
            .WithDescription("Creates application user")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<CreateUserResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.Users.CreateWithRole, async (CreateUserWithRoleCommand request, IMediator mediator, CancellationToken token) =>
            {
                var createUserResult = await mediator.Send(request, token);

                var userManagementLog = new CreateModifyAccessLogCommand
                {
                    UserId = createUserResult.Id,
                    Action = "Add",
                    NewAccess = ApplicationRoles.GetDisplayRoleName(request.Role)
                };
                await mediator.Send(userManagementLog, token);

                var result = new BaseApiResponse<CreateUserWithRoleResult>(createUserResult);

                return Results.Ok(result);
            })
            .WithName("CreateUserWithRole")
            .WithTags(TagName)
            .WithDescription("Creates application user with role")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<CreateUserWithRoleResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapDelete(ApiEndpoints.Users.Delete, async ([FromRoute] Guid id, IMediator mediator, CancellationToken token) =>
            {
                var removeUserResult = await mediator.Send(new RemoveUserCommand(id), token);

                var userManagementLog = new CreateModifyAccessLogCommand
                {
                    UserId = removeUserResult.Id,
                    Action = "Delete"
                };
                await mediator.Send(userManagementLog, token);

                var result = new BaseApiResponse<RemoveUserResult>(removeUserResult);

                return Results.Ok(result);
            })
            .WithName("DeleteUser")
            .WithTags(TagName)
            .WithDescription("Deletes application user")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<RemoveUserResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPut(ApiEndpoints.Users.Update, async ([FromRoute] Guid id, UpdateUserCommand request, IMediator mediator, CancellationToken token) =>
            {
                request.UserId = id;
                var updateUserResult = await mediator.Send(request, token);

                var userManagementLog = new CreateModifyAccessLogCommand
                {
                    UserId = updateUserResult.Id,
                    Action = "Modify Access",
                    CurrentAccess = updateUserResult.OldAccess,
                    NewAccess = updateUserResult.UserAccess
                };
                await mediator.Send(userManagementLog, token);

                var result = new BaseApiResponse<UpdateUserResult>(updateUserResult);

                return Results.Ok(result);
            })
            .WithName("UpdateUser")
            .WithTags(TagName)
            .WithDescription("Updates application user info")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<UpdateUserResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.Users.Query, async (DataSourceRequest request, IMediator mediator, CancellationToken token) =>
            {
                var usersQuery = new GetUsersQuery(request);
                var dataSourceResult = await mediator.Send(usersQuery, token);

                return Results.Ok(dataSourceResult);
            })
            .WithName("QueryUsers")
            .WithTags(TagName)
            .WithDescription("Queries application user")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<DataSourceResult>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapGet(ApiEndpoints.Users.GetSignature, async (IMediator mediator, CancellationToken token) =>
        {
            var command = new GetSignatureRequest();
            var result = await mediator.Send(command, token);

            return Results.Ok(result);
        })
            .WithName("GetUserSignature")
            .WithTags(TagName)
            .WithDescription("Get user signature")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<string>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.Users.Deactivate, async (DeactivateUserCommand request, IMediator mediator, CancellationToken token) =>
            {
                var deactivateResult = await mediator.Send(request, token);

                var userManagementLog = new CreateModifyAccessLogCommand
                {
                    UserId = deactivateResult.UserId,
                    Action = "Deactivate",
                    CurrentAccess = deactivateResult.OldAccess,
                    NewAccess = deactivateResult.NewAccess
                };
                await mediator.Send(userManagementLog, token);

                var result = new BaseApiResponse<DeactivateUserResult>(deactivateResult);

                return Results.Ok(result);
            })
            .WithName("DeactivateUser")
            .WithTags(TagName)
            .WithDescription("Deactivate user application user")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0)
            .Produces<BaseApiResponse<DeactivateUserResult>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        app.MapPost(ApiEndpoints.Users.UploadSignature, async (
               [AsParameters] UploadSignatureRequest model,
               IMediator mediator,
               IBlobService blobService,
               CancellationToken token,
               ILogger<Program> logger) =>
        {
            var request = new
            {
                Id = Guid.NewGuid(),
                File = model.Signature,
                Filename = model.Signature?.FileName
            };

            var metadata = new Dictionary<string, string>();

            byte[] data;
            using (var memoryStream = new MemoryStream())
            {
                await model.Signature.CopyToAsync(memoryStream);
                data = memoryStream.ToArray();
            }

            metadata.Add("DocumentType", "Signature");

            var uri = await blobService.Put(WellKnownContainers.UserDocuments, request.Id.ToString(), data, request.File.ContentType, metadata);

            var command = new UpsertSignatoryUriRequest(uri);

            var response = await mediator.Send(command, token);
            return TypedResults.Ok(new UpdateResponse(response));
        })
           .WithName("UploadSignature")
           .WithTags(TagName)
           .WithDescription("Upload manager signatory")
           .DisableAntiforgery()
           .Produces<UpdateResponse>(StatusCodes.Status200OK)
           .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest);

        return app;
    }
}