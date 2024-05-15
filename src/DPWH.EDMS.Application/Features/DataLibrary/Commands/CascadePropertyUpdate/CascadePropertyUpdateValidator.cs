using FluentValidation;

namespace DPWH.EDMS.Application.Features.DataLibrary.Commands.CascadePropertyUpdate;

public sealed class CascadePropertyUpdateValidator : AbstractValidator<CascadePropertyUpdateCommand>
{
    public CascadePropertyUpdateValidator()
    {
        RuleFor(command => command.UpdateDataLibraryResult)
            .NotNull()
            .WithMessage("Data Library update result should not be null.")
            .ChildRules(v =>
            {
                v.RuleFor(u => u.Type)
                    .IsInEnum()
                    .WithMessage("Type should one of DataLibraryTypes.");

                v.RuleFor(u => u.OldValue)
                    .NotEmpty()
                    .WithMessage("Previous value should not be null or empty.");

                v.RuleFor(u => u.Value)
                    .NotEmpty()
                    .WithMessage("New value should not be null or empty.");
            });
    }
}