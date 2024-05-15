using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Maintenance.Mappers;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequestById;

public record GetMaintenanceRequestByIdQuery(Guid Id) : IRequest<GetMaintenanceRequestByIdResult>;

internal sealed class GetMaintenanceRequestByIdHandler : IRequestHandler<GetMaintenanceRequestByIdQuery, GetMaintenanceRequestByIdResult>
{
    private readonly IReadRepository _repository;

    public GetMaintenanceRequestByIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetMaintenanceRequestByIdResult> Handle(GetMaintenanceRequestByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.MaintenanceRequestsView
            .Include(x => x.Documents)
            .Include(x => x.Asset)
            .Include(x => x.MaintenanceRequestBuildingComponents)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            throw new AppException("Maintenance Request not found.");
        }

        return new GetMaintenanceRequestByIdResult(MaintenanceRequestMappers.MapToModel(entity));
    }
}
