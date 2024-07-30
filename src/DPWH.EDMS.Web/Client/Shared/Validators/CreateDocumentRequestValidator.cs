using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.Enums;
using DPWH.EDMS.Shared.Enums;
using FluentValidation;
using System;

namespace DPWH.EDMS.Web.Client.Shared.Validators;

public class CreateDocumentRequestModelValidator : AbstractValidator<CreateRecordRequest>
{
    public CreateDocumentRequestModelValidator()
    {
        RuleFor(x => x.RequestedRecords)
            .NotEmpty()
            .WithMessage("At least one DPWH issuance or at least one employee record must be selected.");

        RuleFor(x => x.DateRequested)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Date Requested cannot be in the future.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Employee Name is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Employee Email is required.")
            .EmailAddress();

        RuleFor(x => x.Claimant)
            .NotEmpty()
            .WithMessage("Document Claimant is invalid.");

        RuleFor(x => x.AuthorizedRepresentative)
            .NotEmpty()
            .WithMessage("Authorized Representative name is required.")
            .When(x => x != null && x.Claimant == ClaimantTypes.AuthorizedRepresentative.ToString());

        RuleFor(x => x.Purpose)
            .NotEmpty()
            .WithMessage("Purpose is required.");

        RuleFor(x => x.OtherPurpose)
            .NotEmpty()
            .WithMessage("For Other Purpose, you need to specify the reason.")
            .When(x => x != null && x.Purpose != null && x.Purpose.ToLower() == Purposes.Other.ToString().ToLower());

        RuleFor(x => x.Remarks);
    }
}
