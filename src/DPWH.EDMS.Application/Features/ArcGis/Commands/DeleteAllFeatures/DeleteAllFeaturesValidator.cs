using FluentValidation;

namespace DPWH.EDMS.Application.Features.ArcGis.Commands.DeleteAllFeatures;

public class DeleteAllFeaturesValidator : AbstractValidator<DeleteAllFeaturesCommand>
{
    public DeleteAllFeaturesValidator()
    {
        RuleFor(command => command.ServiceName)
            .NotEmpty()
            .WithMessage("ServiceName can't be empty or null.");

        RuleFor(command => command.LayerId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Invalid layer id.");
    }
}

