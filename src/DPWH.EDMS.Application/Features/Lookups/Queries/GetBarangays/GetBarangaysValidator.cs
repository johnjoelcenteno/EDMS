using FluentValidation;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetBarangays;

public sealed class GetBarangaysValidator : AbstractValidator<GetBarangaysQuery>
{
    public GetBarangaysValidator()
    {
        RuleFor(query => query.CityCode)
            .NotEmpty()
            .WithMessage("City or Municipality code is required.");
    }
}