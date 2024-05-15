using FluentValidation;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsByRegionAndYear;

public sealed class GetAssetsByRegionAndYearValidator : AbstractValidator<GetAssetsByRegionAndYearQuery>
{
    public GetAssetsByRegionAndYearValidator()
    {
        RuleFor(query => query.RegionId)
            .NotEmpty()
            .WithMessage("RegionId must not be empty or default.");

        RuleFor(query => query.Year)
            .NotEmpty()
            .WithMessage("Year must not be empty or default.")
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage("Year must not exceed current year")
            .GreaterThanOrEqualTo(DateTime.MinValue.Year)
            .WithMessage($"Year must not be lower than `{DateTime.MinValue.Year}`");
    }
}