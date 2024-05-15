using FluentValidation;

namespace DPWH.EDMS.Application.Features.ArcGis.Queries.FeatureServiceLayer;

public class FeatureServiceLayerValidator : AbstractValidator<FeatureServiceLayerQuery>
{
    public FeatureServiceLayerValidator()
    {
        RuleFor(command => command.ServiceName)
            .NotEmpty()
            .WithMessage("ServiceName can't be empty or null.");

        RuleFor(command => command.LayerId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Invalid layer id.");

        RuleFor(command => command.Where)
            .NotEmpty()
            .WithMessage("Where clause can't be empty or null.");
    }
}