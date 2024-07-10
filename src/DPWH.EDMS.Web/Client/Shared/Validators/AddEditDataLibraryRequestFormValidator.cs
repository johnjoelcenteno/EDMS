using DPWH.EDMS.Client.Shared.Models;
using DPWH.EDMS.Web.Client.Pages.DataLibrary.Common.Enum;
using FluentValidation;

namespace DPWH.EDMS.Web.Client.Shared.Validators;

public class AddEditDataLibraryRequestFormValidator: AbstractValidator<ConfigModel>
{
    public AddEditDataLibraryRequestFormValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Please Specify Value");

        RuleFor(x => x.Office)
            .NotEmpty()
            .WithMessage("Please Specify Office")
            .When(x => x.DataType == DataLibraryEnum.EmployeeRecords.ToString());

        RuleFor(x => x.Section)
           .NotEmpty()
           .WithMessage("Please Specify Section")
           .When(x => x.DataType == DataLibraryEnum.EmployeeRecords.ToString());
    }
}

