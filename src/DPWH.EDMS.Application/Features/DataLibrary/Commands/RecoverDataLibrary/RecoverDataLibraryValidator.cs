using FluentValidation;

namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.RecoverDataLibrary;

public sealed class RecoverDataLibraryValidator : AbstractValidator<RecoverDataLibraryCommand>
{
    public RecoverDataLibraryValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Id must not be empty.");
    }
}