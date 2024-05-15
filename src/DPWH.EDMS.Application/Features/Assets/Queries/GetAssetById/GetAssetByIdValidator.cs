using FluentValidation;

namespace DPWH.EDMS.Application.Features.Assets.Queries.GetAssetById;

public sealed class GetAssetByIdValidator : AbstractValidator<GetAssetByIdQuery>
{
    public GetAssetByIdValidator()
    {
        RuleFor(query => query.AssetId)
            .NotEmpty()
            .WithMessage("Asset Id is required.");
    }
}