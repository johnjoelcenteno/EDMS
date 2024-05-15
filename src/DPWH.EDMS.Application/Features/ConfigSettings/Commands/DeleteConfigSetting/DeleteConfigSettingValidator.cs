using FluentValidation;

namespace DPWH.EDMS.Application.Features.ConfigSettings.Commands.DeleteConfigSetting;

public sealed class DeleteConfigSettingValidator : AbstractValidator<DeleteConfigSettingCommand>
{
    public DeleteConfigSettingValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("Id should not be empty or default");
    }
}