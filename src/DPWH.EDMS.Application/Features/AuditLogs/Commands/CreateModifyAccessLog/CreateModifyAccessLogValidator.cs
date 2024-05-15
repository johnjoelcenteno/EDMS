using FluentValidation;

namespace DPWH.EDMS.Application.Features.AuditLogs.Commands.CreateModifyAccessLog;

public sealed class CreateModifyAccessLogValidator : AbstractValidator<CreateModifyAccessLogCommand>
{
    public CreateModifyAccessLogValidator()
    {
        RuleFor(command => command.Action)
            .NotEmpty()
            .WithMessage("Action must not be empty or null.");

        RuleFor(command => command.UserId)
            .NotEmpty()
            .WithMessage("UserId must not be empty or default.");
    }
}