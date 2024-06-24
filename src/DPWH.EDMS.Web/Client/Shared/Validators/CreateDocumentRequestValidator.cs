using DPWH.EDMS.Api.Contracts;
using DPWH.EDMS.Client.Shared.MockModels;
using DPWH.EDMS.Shared.Enums;
using FluentValidation;

namespace DPWH.EDMS.Web.Client.Shared.Validators;

public class CreateDocumentRequestModelValidator : AbstractValidator<CreateRecordRequest>
{
    public CreateDocumentRequestModelValidator()
    {
        //RuleFor(x => x.ControlNumber)
        //    .NotEmpty().WithMessage("Control Number is required.")
        //    .MaximumLength(50).WithMessage("Control Number must not exceed 50 characters.");

        RuleFor(x => x.RequestedRecords)
            .NotEmpty().WithMessage("At least one record must be requested.");

        RuleFor(x => x.DateRequested)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date Requested cannot be in the future.");

        //RuleFor(x => x.Purpose)
        //    .NotEmpty().WithMessage("Purpose is required.")
        //    .MaximumLength(250).WithMessage("Purpose must not exceed 250 characters.");

        //RuleFor(x => x.Status)
        //    .NotEmpty().WithMessage("Status is required.")
        //    .MaximumLength(50).WithMessage("Status must not exceed 50 characters.");

        //RuleFor(x => x.EmployeeNumber)
        //    .NotEmpty().WithMessage("Employee Number is required.")
        //    .MaximumLength(20).WithMessage("Employee Number must not exceed 20 characters.");

        RuleFor(x => x.Claimant)
            .NotEmpty().WithMessage("Document Claimant is invalid.");

        RuleFor(x => x.AuthorizedRepresentative)
            .MaximumLength(100).WithMessage("Authorized Representative Name must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.AuthorizedRepresentative));

        //RuleFor(x => x.ValidId)
        //    .NotEmpty().WithMessage("Valid ID is required.")
        //    .When(x => x.Claimant == ClaimantTypes.AuthorizedRepresentative.ToString());
        //.MaximumLength(50).WithMessage("Valid ID Type must not exceed 50 characters.");

        //RuleFor(x => x.SupportingDocument)
        //.NotEmpty().WithMessage("Supporting Document Type is required.")
        //.MaximumLength(50).WithMessage("Supporting Document Type must not exceed 50 characters.");

        RuleFor(x => x.Purpose)
            .NotEmpty().WithMessage("Purpose/Remarks is required.");

        //RuleFor(x => x.IsValidIdAccepted)
        //    .Equal(true)
        //    .WithMessage("Please upload a valid ID - PDF or DOCX files only");

        //RuleFor(x => x.IsSupportedDocumentAccepted)
        //    .Equal(true)
        //    .WithMessage("Please upload a valid supporting document - PDF or DOCX files only");
    }
}