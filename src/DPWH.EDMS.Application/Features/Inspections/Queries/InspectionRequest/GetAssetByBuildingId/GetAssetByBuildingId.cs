using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Assets.Mappers;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Inspections.Queries.InspectionRequest.GetAssetByBuildingId;

public record GetAssetByBuildingIdQuery(string BuildingId, string? Purpose) : IRequest<GetAssetByBuildingIdWithBuildingComponentResult>;

internal sealed class GetAssetByBuildingId : IRequestHandler<GetAssetByBuildingIdQuery, GetAssetByBuildingIdWithBuildingComponentResult>
{
    private readonly IReadRepository _repository;
    public GetAssetByBuildingId(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetAssetByBuildingIdWithBuildingComponentResult> Handle(GetAssetByBuildingIdQuery request, CancellationToken cancellationToken)
    {
        List<ProjectMonitoring>? projectMonitorings = null;
        List<MaintenanceRequest>? maintenanceRequests = null;
        IEnumerable<string> requestNumbers = [];

        var asset = _repository.AssetsView.FirstOrDefault(x => x.BuildingId == request.BuildingId)
            ?? throw new AppException("Asset not found");

        if (request.Purpose == "Priority List Inspection")
        {
            maintenanceRequests = await _repository.MaintenanceRequestsView
                .Include(x => x.MaintenanceRequestBuildingComponents)
                .Where(x => x.AssetId == asset.Id)
                .ToListAsync(cancellationToken)
                ?? throw new AppException("No maintenance requests found");

            requestNumbers = maintenanceRequests
                .Select(x => x.RequestNumber).ToList();

        }
        else if (request.Purpose == "Project Monitoring")
        {
            projectMonitorings = await _repository.ProjectMonitoringView
                .Include(x => x.ProjectMonitoringBuildingComponents)
                .Where(x => x.AssetId == asset.Id)
                .ToListAsync(cancellationToken)
                ?? throw new AppException("No project monitorings found");

            requestNumbers = projectMonitorings
                .Select(x => x.ContractId).ToList();
        }

        var model = AssetMappers.MapToModel(asset);
        return new GetAssetByBuildingIdWithBuildingComponentResult(model, requestNumbers);
    }
}
