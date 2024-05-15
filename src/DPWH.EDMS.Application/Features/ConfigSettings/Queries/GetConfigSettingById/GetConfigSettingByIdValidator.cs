using FluentValidation;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Queries.GetConfigSettingById;

public sealed class GetConfigSettingByIdValidator : AbstractValidator<GetConfigSettingByIdQuery>
{
    public GetConfigSettingByIdValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .WithMessage("Id must not be empty or default.");
    }
}