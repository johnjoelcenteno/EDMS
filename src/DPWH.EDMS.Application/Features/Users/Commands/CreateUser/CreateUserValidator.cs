using FluentValidation;

namespace DPWH.EDMS.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
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
    }
}