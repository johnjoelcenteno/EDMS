using System.Security.Claims;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.IDP.Core.Constants;
using DPWH.EDMS.IDP.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DPWH.EDMS.Application.Features.Users.Commands.DeactivateUser;

public record DeactivateUserCommand(Guid UserId, string Reason) : IRequest<DeactivateUserResult>;

internal sealed class DeactivateUserHandler : IRequestHandler<DeactivateUserCommand, DeactivateUserResult>
{
    private readonly ILogger<DeactivateUserHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public DeactivateUserHandler(ILogger<DeactivateUserHandler> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<DeactivateUserResult> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            _logger.LogError("User `{UserId}` not found", request.UserId);
            throw new AppException($"User `{request.UserId}` not found");
        }

        //remove user's existing role claim
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roleClaims = userClaims.Where(u => u.Type == "role").ToList();
        if (roleClaims.Any())
        {
            var removeClaimResult = await _userManager.RemoveClaimsAsync(user, roleClaims);
            if (!removeClaimResult.Succeeded)
            {
                var removeClaimError = removeClaimResult.Errors.First().Description;
                _logger.LogError("Failed to remove role claim from user `{Email}`: {Error}", user.Email, removeClaimError);
                throw new AppException($"Failed to remove role claim from user `{user.EmployeeInfo.EmployeeId}`: {removeClaimError}");
            }
        }

        //add account_status claim flag
        var deactivatedClaim = new Claim("role", "deactivated");
        var addDeactivatedClaimResult = await _userManager.AddClaimAsync(user, deactivatedClaim);
        if (!addDeactivatedClaimResult.Succeeded)
        {
            var addClaimError = addDeactivatedClaimResult.Errors.First().Description;
            _logger.LogError("Failed to add deactivated claim to user `{Email}`: {Error}", user.Email, addClaimError);
            throw new AppException($"Failed to add deactivated claim to user `{user.Email}`: {addClaimError}");
        }

        return new DeactivateUserResult
        {
            UserId = user.Id,
            OldAccess = ApplicationRoles.GetDisplayRoleName(roleClaims.FirstOrDefault()?.Value, "None"),
            NewAccess = ApplicationRoles.GetDisplayRoleName(deactivatedClaim.Value)
        };
    }
}