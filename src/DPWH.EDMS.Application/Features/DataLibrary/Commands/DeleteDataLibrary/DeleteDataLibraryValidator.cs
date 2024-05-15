using FluentValidation;

namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.DeleteDataLibrary;

public sealed class DeleteDataLibraryValidator : AbstractValidator<DeleteDataLibraryCommand>
{
    public DeleteDataLibraryValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Id must not be empty.");
    }
}