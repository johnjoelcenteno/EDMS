using DPWH.EDMS.IDP.Core.Constants;
using FluentValidation;

namespace DPWH.EDMS.Application.Features.Users.Commands.CreateUserWithRole;

public sealed class CreateUserWithRoleValidator : AbstractValidator<CreateUserWithRoleCommand>
{
    public CreateUserWithRoleValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .WithMessage("Email can't be empty or null.")
            .EmailAddress()
            .WithMessage("Invalid email address format.");

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