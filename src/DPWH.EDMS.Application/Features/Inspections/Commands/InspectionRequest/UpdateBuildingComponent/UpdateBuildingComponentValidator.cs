using FluentValidation;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.UpdateBuildingComponent;

public sealed class UpdateBuildingComponentValidator : AbstractValidator<UpdateBuildingComponentCommand>
{
    public UpdateBuildingComponentValidator()
    {
        RuleFor(command => command.SubCategories)
            .NotNull()
            .WithMessage("Request object must not be null");

        RuleForEach(command => command.SubCategories)
            .ChildRules(subCategory =>
            {
                subCategory.RuleFor(x => x.SubCategory)
                    .NotEmpty()
                    .WithMessage("SubCategory must not be null or empty");
            });
    }
}
