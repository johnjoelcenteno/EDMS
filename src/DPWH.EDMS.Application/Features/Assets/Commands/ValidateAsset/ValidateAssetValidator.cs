using FluentValidation;

namespace DPWH.EDMS.Application.Features.Assets.Commands.ValidateAsset;

internal class ValidateAssetValidator : AbstractValidator<ValidateAssetCommand>
{
    public ValidateAssetValidator()
    {
        RuleFor(command => command.ValidateAssetRequest)
            .NotNull()
            .WithMessage("Request object must not be null.")
            .ChildRules(v =>
            {
                v.RuleFor(param => param.Id)
                    .NotEmpty()
                    .WithMessage("Asset Id must not be empty or default.");

                v.RuleFor(param => param.BuildingId)
                    .NotEmpty()
                    .WithMessage("Building Id must not be empty or null.");

                v.RuleFor(param => param.Name)
                    .NotEmpty()
                    .WithMessage("Name must not be empty or null.");

                v.RuleFor(param => param.RequestingOffice)
                    .NotEmpty()
                    .WithMessage("Requesting Office must not be empty or null.");

                v.RuleFor(param => param.ImplementingOffice)
                    .NotEmpty()
                    .WithMessage("Implementing Office must not be empty or null.");

                v.RuleFor(param => param.Agency)
                    .NotEmpty()
                    .WithMessage("Agency must not be empty or null.");

                v.RuleFor(param => param.AttachedAgency)
                    .NotEmpty()
                    .WithMessage("Attached Agency/Office must not be empty or null.");

                v.RuleFor(param => param.Group)
                    .NotEmpty()
                    .WithMessage("Group must not be empty or null.");

                v.RuleFor(param => param.Region)
                    .NotEmpty()
                    .WithMessage("Region (Location) must not be empty or null.");

                v.RuleFor(param => param.StreetAddress)
                    .NotEmpty()
                    .WithMessage("Street Address must not be empty or null.");

                v.RuleFor(param => param.PropertyStatus)
                    .NotEmpty()
                    .WithMessage("Property Condition must not be empty or null.");

                v.RuleFor(param => param.BuildingStatus)
                    .NotEmpty()
                    .WithMessage("Building Status must not be empty or null.");

                v.RuleFor(param => param.FloorArea)
                    .NotEmpty()
                    .WithMessage("Floor Area must not be empty or null.");

                v.RuleFor(param => param.Floors)
                    .NotEmpty()
                    .WithMessage("No. of Storey must not be empty or null.");

                v.RuleFor(param => param.ConstructionType)
                    .NotEmpty()
                    .WithMessage("Construction Type must not be empty or null.");

                v.RuleFor(param => param.YearConstruction)
                    .NotEmpty()
                    .WithMessage("Year of Construction must not be empty or null.");

                v.RuleFor(param => param.MonthConstruction)
                    .Must(month => month is null or > 0 and <= 12)
                    .WithMessage("Month of Construction may be empty or within the range of 1 to 12.");

                v.RuleFor(param => param.DayConstruction)
                    .Must(day => day is null or > 0 and <= 31)
                    .WithMessage("Day of Construction may be empty or within the range of 1 to 31.");

                v.RuleFor(param => param.AppraisedValue)
                    .NotEmpty()
                    .WithMessage("Appraised Value must not be empty or null.");

                v.RuleFor(param => param.BookValue)
                    .NotEmpty()
                    .WithMessage("Book Value must not be empty or null.");

                v.RuleFor(param => param.LotStatus)
                    .NotEmpty()
                    .WithMessage("Lot Status must not be empty or null.");

                v.RuleFor(param => param.LotArea)
                    .NotEmpty()
                    .WithMessage("Lot Area must not be empty or null.");

                v.RuleFor(param => param.ZonalValue)
                    .NotEmpty()
                    .WithMessage("BIR Zonal Value must not be empty or null.");
            });
    }
}
