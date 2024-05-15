using FluentValidation;

namespace DPWH.EDMS.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(query => query.UserId)
            .NotEqual(Guid.Empty)
            .WithMessage("The user id can't be an empty guid.");
    }
}