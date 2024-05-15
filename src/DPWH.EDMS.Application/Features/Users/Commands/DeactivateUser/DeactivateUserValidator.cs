using FluentValidation;

namespace DPWH.EDMS.Application.Features.Users.Commands.DeactivateUser;

public sealed class DeactivateUserValidator : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserValidator()
    {
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("The user id can't be an empty guid.");

        RuleFor(command => command.Reason)
            .NotEmpty()
            .WithMessage("Reason must not be empty.");
    }
}