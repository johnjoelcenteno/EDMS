using FluentValidation;

namespace DPWH.EDMS.Application.Features.RecordsManagement.Commands.CreateRecord;
public sealed class CreateRecordValidator : AbstractValidator<CreateRecordCommand>
{
    public CreateRecordValidator()
    {
        RuleFor(command => command)
            .NotNull()
            .WithMessage("Request object must not be null.")
            .ChildRules(v =>
            {
                v.RuleFor(param => param.RecordTypeId)
                    .NotEmpty()
                    .WithMessage("RecordTypeId must not be empty or null.");

                v.RuleFor(param => param.RecordName)
                    .NotEmpty()
                    .WithMessage("RecordName must not be empty or null.");

                v.RuleFor(param => param.EmployeeId)
                    .NotEmpty()
                    .WithMessage("EmployeeId must not be empty or null.");
            });
    }
}
