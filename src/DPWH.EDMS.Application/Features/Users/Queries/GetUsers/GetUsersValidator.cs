using FluentValidation;

namespace DPWH.EDMS.Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersValidator()
    {
        RuleFor(query => query.DataSourceRequest)
            .NotNull()
            .WithMessage("Query request object must not be null.");
    }
}