using DPWH.EDMS.Components.Helpers;
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
        RuleFor(x => x.Code)
           .NotEmpty().WithMessage("Please Specify Code")
           .When(item => item.Category == DataLibraryEnum.PersonalRecords.GetDescription());

        RuleFor(x => x.Office)
            .NotEmpty()
            .WithMessage("Please Specify Office")
            .When(item => item.Category != DataLibraryEnum.PersonalRecords.GetDescription());

        RuleFor(x => x.Section)
           .NotEmpty()
           .WithMessage("Please Specify Section")
            .When(item => item.Category != DataLibraryEnum.PersonalRecords.GetDescription()); ;
    }
}
