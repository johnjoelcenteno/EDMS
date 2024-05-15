using FluentValidation;

namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.AddDataLibrary;

public sealed class AddDataLibraryValidator : AbstractValidator<AddDataLibraryCommand>
{
    public AddDataLibraryValidator()
    {
        RuleFor(command => command.Type)
            .IsInEnum()
            .WithMessage("Type should one of DataLibraryTypes.");

        RuleFor(command => command.Value)
            .NotEmpty()
            .WithMessage("Value should not be null or empty.");
    }
}