using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Maintenance.Mappers;
using DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequestById;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequestByBuildingId;

public record GetMaintenanceRequestByBuildingIdQuery(string Id) : IRequest<GetMaintenanceRequestByIdResult>;

internal sealed class GetMaintenanceRequestByBuildingIdHandler : IRequestHandler<GetMaintenanceRequestByBuildingIdQuery, GetMaintenanceRequestByIdResult>
{
    private readonly IReadRepository _repository;

    public GetMaintenanceRequestByBuildingIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetMaintenanceRequestByIdResult> Handle(GetMaintenanceRequestByBuildingIdQuery request, CancellationToken cancellationToken)
    {

        var asset = _repository.AssetsView.FirstOrDefault(x => x.BuildingId == request.Id);

        var entity = await _repository.MaintenanceRequestsView
            .Include(x => x.Documents)
            .Include(x => x.Asset)
            .Include(x => x.MaintenanceRequestBuildingComponents)
            .FirstOrDefaultAsync(x => x.AssetId == asset.Id, cancellationToken)
            ?? throw new AppException("Maintenance Request not found.");

        return new GetMaintenanceRequestByIdResult(MaintenanceRequestMappers.MapToModel(entity));
    }
}
