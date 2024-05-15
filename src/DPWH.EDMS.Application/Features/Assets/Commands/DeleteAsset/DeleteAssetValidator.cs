using FluentValidation;

namespace DPWH.EDMS.Application.Features.Assets.Commands.DeleteAsset;

public sealed class DeleteAssetValidator : AbstractValidator<DeleteAssetCommand>
{
    public DeleteAssetValidator()
    {
        RuleFor(command => command.AssetId)
            .NotEmpty()
            .WithMessage("Asset ID is required.");
    }
}