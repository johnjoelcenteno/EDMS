using FluentValidation;

namespace DPWH.EDMS.Application.Features.AuditLogs.Queries.GetAuditLogByEntityId;

public sealed class GetAuditLogByEntityIdValidator : AbstractValidator<GetAuditLogByEntityIdQuery>
{
    public GetAuditLogByEntityIdValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .WithMessage("Entity Id must not be null or empty.");
    }
}