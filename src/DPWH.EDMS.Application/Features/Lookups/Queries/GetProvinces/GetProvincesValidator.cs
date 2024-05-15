using FluentValidation;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetProvinces;

public sealed class GetProvincesValidator : AbstractValidator<GetProvincesQuery>
{
    public GetProvincesValidator()
    {
        RuleFor(query => query.RegionCode)
            .NotEmpty()
            .WithMessage("Region code is required.");
    }
}