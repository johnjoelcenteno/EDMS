using Microsoft.AspNetCore.Authorization;

namespace DPWH.EDMS.Api.Middlewares;

public class DevelopmentModeAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
