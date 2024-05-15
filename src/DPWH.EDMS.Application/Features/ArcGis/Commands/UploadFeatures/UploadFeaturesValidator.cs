using FluentValidation;
namespace DPWH.EDMS.Application.Features.ArcGis.Commands.UploadFeatures;
public class UploadFeaturesValidator : AbstractValidator<UploadFeaturesCommand>
{
    public UploadFeaturesValidator()
    {
        RuleFor(command => command.ServiceName)
            .NotEmpty()
            .WithMessage("ServiceName can't be empty or null.");

        RuleFor(command => command.LayerId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Invalid layer id.");

        RuleFor(command => command.Features)
            .NotEmpty()
            .WithMessage("Features can't be empty or null.");
    }
}
