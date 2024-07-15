using DPWH.EDMS.Web.Client.Pages.DataLibrary.Common.Enum;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.RecordTypes.Common.Model;
using FluentValidation;

namespace DPWH.EDMS.Web.Client.Shared.Validators;

public class AddEditRecordTypeFormValidator : AbstractValidator<RecordsLibraryModel>
{
    public AddEditRecordTypeFormValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Please Specify Value");

        RuleFor(x => x.Office)
            .NotEmpty()
            .WithMessage("Please Specify Office");

        RuleFor(x => x.Section)
           .NotEmpty()
           .WithMessage("Please Specify Section");
    }
}
