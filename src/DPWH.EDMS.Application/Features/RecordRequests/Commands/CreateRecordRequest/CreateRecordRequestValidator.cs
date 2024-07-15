using DPWH.EDMS.Shared.Enums;
using FluentValidation;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.CreateRecordRequest;
public sealed class CreateRecordRequestValidator : AbstractValidator<CreateRecordRequestCommand>
{
    public CreateRecordRequestValidator()
    {
        RuleFor(command => command.Model)
            .NotNull()
            .WithMessage("Request object must not be null.")
            .ChildRules(v =>
            {
                v.RuleFor(param => param.Purpose)
                    .NotEmpty()
                    .WithMessage("Purpose must not be empty or null.");

                v.RuleFor(param => param.Claimant)
                    .Must(i => Enum.IsDefined(typeof(ClaimantTypes), i))
                    .WithMessage("Claimant must be either Requestor or AuthorizedRepresentative");

                v.RuleFor(param => param.RequestedRecords)
                    .NotEmpty()
                    .WithMessage("RequestedRecords must not be empty or null.");

                v.RuleFor(param => param.SupportingFileValidId)                    
                    .NotEmpty()
                    .When(x => x.Claimant == ClaimantTypes.AuthorizedRepresentative.ToString())
                    .WithMessage("SupportingFileValidId must not be empty or null when claimant is representative.");

            });
    }
}
