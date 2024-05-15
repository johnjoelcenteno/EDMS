using FluentValidation;

namespace DPWH.EDMS.Application.Features.Lookups.Queries.GetCities;

public sealed class GetCitiesValidator : AbstractValidator<GetCitiesQuery>
{
    public GetCitiesValidator()
    {
        RuleFor(query => query.ProvinceCode)
            .NotEmpty()
            .WithMessage("Province code is required.");
    }
}