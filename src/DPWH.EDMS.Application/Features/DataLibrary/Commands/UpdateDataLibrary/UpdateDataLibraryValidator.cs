using FluentValidation;

namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.UpdateDataLibrary;

public sealed class UpdateDataLibraryValidator : AbstractValidator<UpdateDataLibraryCommand>
{
    public UpdateDataLibraryValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Data library id should not be empty.");

        RuleFor(command => command.Value)
            .NotEmpty()
            .WithMessage("Value should not be empty.");
    }
}