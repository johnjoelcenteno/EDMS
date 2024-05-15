using FluentValidation;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Commands.UpdateConfigSetting;

public sealed class UpdateConfigSettingValidator : AbstractValidator<UpdateConfigSettingCommand>
{
    public UpdateConfigSettingValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Id must not be empty or default.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty or null");

        RuleFor(command => command.Value)
            .NotEmpty()
            .WithMessage("Value must not be empty or null");
    }
}