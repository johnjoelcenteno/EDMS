using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.AuditLogs.Queries.GetAuditLogByEntityId;

public record GetAuditLogByEntityIdQuery(string Id) : IRequest<IEnumerable<GetAuditLogByEntityIdResult>>;

internal sealed class GetAuditLogByEntityIdHandler : IRequestHandler<GetAuditLogByEntityIdQuery, IEnumerable<GetAuditLogByEntityIdResult>>
{
    private readonly IReadRepository _repository;

    public GetAuditLogByEntityIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GetAuditLogByEntityIdResult>> Handle(GetAuditLogByEntityIdQuery request, CancellationToken cancellationToken)
    {
        var auditLogs = await _repository.ChangeLogsView
            .Include(log => log.Changes)
            .Where(log => log.EntityId == request.Id)
            .OrderByDescending(log => log.ActionDate)
            .ToListAsync(cancellationToken);

        return auditLogs.Select(log => new GetAuditLogByEntityIdResult
        {
            Changes = log.Changes.Select(l => new GetAuditLogByEntityIdResultItemChange(l.Field, l.From, l.To)),
            LogDetails = new GetAuditLogByEntityIdResultDetail(log)
        });
    }
}