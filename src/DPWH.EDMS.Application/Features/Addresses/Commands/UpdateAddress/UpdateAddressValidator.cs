using FluentValidation;

namespace DPWH.EDMS.Application.Features.Addresses.Commands.UpdateAddress;

public sealed class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Id must not be empty or default (0).");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Name should not be null or empty.");

        RuleFor(command => command.Type)
            .IsInEnum()
            .WithMessage("Address type must be valid.");
    }
}