using DPWH.EDMS.Api.Contracts;
using FluentValidation;

namespace DPWH.EDMS.Web.Client.Shared.Validators;

public class CreateMenuItemValidator : AbstractValidator<CreateMenuItemModel>
{
    public CreateMenuItemValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text is required.")
            .MaximumLength(100).WithMessage("Text must be less than 100 characters.");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url is required.");

        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("Icon is required.");

        RuleFor(x => x.Expanded)
            .NotNull().WithMessage("Expanded is required.");

        RuleFor(x => x.Level)
            .NotNull().WithMessage("Level is required.");

        RuleFor(x => x.SortOrder)
            .NotNull().WithMessage("SortOrder is required.");

        RuleFor(x => x.AuthorizedRoles)
            .NotNull().WithMessage("AuthorizedRoles is required.")
            .Must(x => x.Count > 0).WithMessage("AuthorizedRoles must contain at least one role.");

        RuleFor(x => x.NavType)
            .NotNull().WithMessage("Nav Type is required.");
    }
}

public class UpdateMenuItemValidator : AbstractValidator<MenuItemModel>
{
    public UpdateMenuItemValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text is required.")
            .MaximumLength(100).WithMessage("Text must be less than 100 characters.");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url is required.");

        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("Icon is required.");

        RuleFor(x => x.Expanded)
            .NotNull().WithMessage("Expanded is required.");

        RuleFor(x => x.Level)
            .NotNull().WithMessage("Level is required.");

        RuleFor(x => x.SortOrder)
            .NotNull().WithMessage("SortOrder is required.");

        RuleFor(x => x.AuthorizedRoles)
            .NotNull().WithMessage("AuthorizedRoles is required.")
            .Must(x => x.Count > 0).WithMessage("AuthorizedRoles must contain at least one role.");

        RuleFor(x => x.NavType)
            .NotNull().WithMessage("Nav Type is required.");
    }
}