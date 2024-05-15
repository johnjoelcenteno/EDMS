using FluentValidation;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Queries.GetConfigSettings;

public sealed class GetConfigSettingsQueryValidator : AbstractValidator<GetConfigSettingsQuery>
{
    public GetConfigSettingsQueryValidator()
    {
        RuleFor(query => query.DataSourceRequest)
            .NotEmpty()
            .WithMessage("Request must not be empty");
    }
}