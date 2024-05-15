using FluentValidation;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Commands.CreateConfigSetting;

public sealed class CreateConfigSettingValidator : AbstractValidator<CreateConfigSettingCommand>
{
    public CreateConfigSettingValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Name should not be empty or null.");

        RuleFor(command => command.Value)
            .NotEmpty()
            .WithMessage("Value should not be empty or null.");
    }
}