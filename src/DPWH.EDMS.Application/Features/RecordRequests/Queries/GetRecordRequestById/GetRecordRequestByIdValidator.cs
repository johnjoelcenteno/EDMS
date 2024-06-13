using FluentValidation;

namespace DPWH.EDMS.Application.Features.RecordRequests.Queries.GetRecordRequestById;

public sealed class GetRecordRequestByIdValidator : AbstractValidator<GetRecordRequestByIdQuery>
{
    public GetRecordRequestByIdValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .WithMessage("RecordRequest Id is required.");
    }
}