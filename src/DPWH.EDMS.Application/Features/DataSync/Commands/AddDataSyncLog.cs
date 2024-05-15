using System.Security.Claims;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.IDP.Core.Extensions;
using MediatR;

namespace DPWH.EDMS.Application.Features.DataSync.Commands;

public record AddDataSyncLog(string DataType, bool IsSuccess, string? Description) : IRequest;

internal sealed class AddDataSyncHandler(IWriteRepository repository, ClaimsPrincipal principal) : IRequestHandler<AddDataSyncLog>
{
    public async Task Handle(AddDataSyncLog request, CancellationToken cancellationToken)
    {
        var result = request.IsSuccess ? "Success" : "Failed";
        var log = DataSyncLog.Create(request.DataType, result, request.Description, principal.GetUserName());

        await repository.DataSyncLogs.AddAsync(log, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}