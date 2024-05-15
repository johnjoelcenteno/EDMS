using DPWH.EDMS.IDP.Core.Constants;
using FluentValidation;

namespace DPWH.EDMS.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("The user id can't be an empty guid.");

        RuleFor(command => command.FirstName)
            .NotEmpty()
            .WithMessage("First name can't be empty or null.");

        RuleFor(command => command.LastName)
            .NotEmpty()
            .WithMessage("Last name can't be empty or null.");

        RuleFor(command => command.Role)
            .NotEmpty()
            .WithMessage("Role can't be empty or null.")
            .Must(role => ApplicationRoles.UserAccessMapping.ContainsKey(role))
            .WithMessage("Role must be one of the registered role on identity.");
    }
}