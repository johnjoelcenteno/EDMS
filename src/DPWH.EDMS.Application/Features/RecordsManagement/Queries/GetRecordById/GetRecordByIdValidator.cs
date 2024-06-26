using FluentValidation;

namespace DPWH.EDMS.Application.Features.RecordsManagement.Queries.GetRecordById;

public sealed class GetRecordByIdValidator : AbstractValidator<GetRecordByIdQuery>
{
    public GetRecordByIdValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}