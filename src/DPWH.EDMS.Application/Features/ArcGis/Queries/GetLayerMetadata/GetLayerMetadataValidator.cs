using FluentValidation;

namespace DPWH.EDMS.Application.Features.ArcGis.Queries.GetLayerMetadata;

public sealed class GetLayerMetadataValidator : AbstractValidator<GetLayerMetadataCommand>
{
    public GetLayerMetadataValidator()
    {
        RuleFor(command => command.ServiceName)
            .NotEmpty()
            .WithMessage("ServiceName can't be empty or null.");

        RuleFor(command => command.LayerId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Invalid layer id.");
    }
}