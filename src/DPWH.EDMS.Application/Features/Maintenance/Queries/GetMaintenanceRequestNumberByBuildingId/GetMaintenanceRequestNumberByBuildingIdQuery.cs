using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;

namespace DPWH.EDMS.Application.Features.Maintenance.Queries.GetMaintenanceRequestNumberByBuildingId;

public record GetMaintenanceRequestNumberByBuildingIdQuery(string BuildingId) : IRequest<GetMaintenanceRequestNumberByBuildingIdResult>;
internal sealed class GetMaintenanceRequestNumberByBuildingIdHandler : IRequestHandler<GetMaintenanceRequestNumberByBuildingIdQuery, GetMaintenanceRequestNumberByBuildingIdResult>
{
    public readonly IReadRepository _repository;

    public GetMaintenanceRequestNumberByBuildingIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetMaintenanceRequestNumberByBuildingIdResult> Handle(GetMaintenanceRequestNumberByBuildingIdQuery request, CancellationToken cancellationToken)
    {
        var asset = _repository.AssetsView.FirstOrDefault(x => x.BuildingId == request.BuildingId);

        var maintenanceRequests = _repository.MaintenanceRequestsView
                                    .Where(x => x.AssetId == asset.Id)
                                    .OrderByDescending(x => x.Created)
                                    .ToList();

        return new GetMaintenanceRequestNumberByBuildingIdResult(maintenanceRequests);
    }
}
