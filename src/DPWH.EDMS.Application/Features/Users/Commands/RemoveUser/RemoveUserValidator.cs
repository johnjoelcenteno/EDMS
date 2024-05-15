using FluentValidation;

namespace DPWH.EDMS.Application.Features.Users.Commands.RemoveUser;

public sealed class RemoveUserValidator : AbstractValidator<RemoveUserCommand>
{
    public RemoveUserValidator()
    {
        RuleFor(command => command.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("The user id can't be an empty guid.");
    }
}