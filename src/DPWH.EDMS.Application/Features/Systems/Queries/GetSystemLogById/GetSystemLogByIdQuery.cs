using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Systems.Mappers;
using DPWH.EDMS.Application.Features.Systems.Queries.Models;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Systems.Queries.GetSystemLogById;

public record GetSystemLogByIdQuery(Guid SystemLogId) : IRequest<SystemLogsResponse>;

internal sealed class GetSystemLogByIdHandler : IRequestHandler<GetSystemLogByIdQuery, SystemLogsResponse>
{
    private readonly IReadRepository _repository;

    public GetSystemLogByIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<SystemLogsResponse> Handle(GetSystemLogByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository
            .SystemLogsView
            .FirstOrDefaultAsync(log => log.Id == request.SystemLogId, cancellationToken);

        if (entity is null)
        {
            throw new AppException($"System Log not found: {request.SystemLogId}");
        }

        var model = SystemLogMappers.MapToModel(entity);
        return new SystemLogsResponse(model);
    }
}