using FluentValidation;

namespace DPWH.EDMS.Application.Features.Assets.Commands.PatchAsset;

public sealed class PatchAssetValidator : AbstractValidator<PatchAssetCommand>
{
    public PatchAssetValidator()
    {
        RuleFor(command => command.AssetId)
            .NotEmpty()
            .WithMessage("Property Id is required.");

        RuleFor(command => command.Asset)
            .NotNull()
            .WithMessage("Empty property details.")
            .ChildRules(param =>
            {
                param.RuleFor(asset => asset.Operations)
                    .NotEmpty()
                    .WithMessage("Empty property details.");
            });
    }
}