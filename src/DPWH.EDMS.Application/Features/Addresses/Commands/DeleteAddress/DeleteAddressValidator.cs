using FluentValidation;

namespace DPWH.EDMS.Application.Features.Addresses.Commands.DeleteAddress;

public sealed class DeleteAddressValidator : AbstractValidator<DeleteAddressCommand>
{
    public DeleteAddressValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Address Id must not be empty or default.");
    }
}