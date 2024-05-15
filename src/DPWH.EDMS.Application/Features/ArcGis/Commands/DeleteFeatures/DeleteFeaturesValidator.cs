using FluentValidation;

namespace DPWH.EDMS.Application.Features.ArcGis.Commands.DeleteFeatures;

public class DeleteFeaturesValidator : AbstractValidator<DeleteFeaturesCommand>
{
    public DeleteFeaturesValidator()
    {
        RuleFor(command => command.ServiceName)
            .NotEmpty()
            .WithMessage("ServiceName can't be empty or null.");

        RuleFor(command => command.LayerId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Invalid layer id.");

        RuleFor(command => command.ObjectIds)
            .NotEmpty()
            .WithMessage("A minimum of one (1) object id must be provided.");
    }
}