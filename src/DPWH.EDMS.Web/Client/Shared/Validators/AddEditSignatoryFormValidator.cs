using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.Common.Enum;
using FluentValidation;

namespace DPWH.EDMS.Web.Client.Shared.Validators;

public class AddEditSignatoryFormValidator : AbstractValidator<SignatoryManagementModel>
{
    public AddEditSignatoryFormValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Please Specify Name");

        RuleFor(x => x.DocumentType)
            .NotEmpty()
            .WithMessage("Please Specify Document Type");

        RuleFor(x => x.Position)
            .NotEmpty()
            .WithMessage("Please Specify Position");

        RuleFor(x => x.Office1)
            .NotEmpty()
            .WithMessage("Please Specify Office 1");

        RuleFor(x => x.Office2)
            .NotEmpty()
            .WithMessage("Please Specify Office 2");

        RuleFor(x => x.SignatoryNo)
           .NotNull()
           .WithMessage("Please Specify Signatory number");
    }
}

