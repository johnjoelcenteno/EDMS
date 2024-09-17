using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

namespace DPWH.EDMS.Application.Features.AuditLogs.Commands.CreateModifyAccessLog;

public record CreateModifyAccessLogCommand : IRequest
{
    public required string Action { get; set; }
    public required string UserId { get; set; }
    public string? CurrentAccess { get; set; }
    public string? NewAccess { get; set; }
}

internal sealed class CreateModifyAccessLogHandler : IRequestHandler<CreateModifyAccessLogCommand>
{
    private readonly IWriteRepository _repository;
    private readonly ClaimsPrincipal _principal;

    public CreateModifyAccessLogHandler(IWriteRepository repository, ClaimsPrincipal principal)
    {
        _repository = repository;
        _principal = principal;
    }

    public async Task Handle(CreateModifyAccessLogCommand request, CancellationToken cancellationToken)
    {
        if (request.CurrentAccess == request.NewAccess) return;

        var change = ChangeLogItem.Create("Access", request.CurrentAccess, request.NewAccess);
        var changeLog = ChangeLog.Create(
            request.UserId,
            "User Management",  
            null,
            null,
            request.Action,
            _principal.GetUserId().ToString(),
            _principal.GetUserName(),
            _principal.GetFirstName(),
            _principal.GetLastName(),
            _principal.GetMiddleInitial(),
            _principal.GetEmployeeNumber(),
            new[] { change });

        await _repository.ChangeLogs.AddAsync(changeLog, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}