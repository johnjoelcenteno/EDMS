using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Mappers;
using DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetProjectMonitoringById;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetProjectMonitoringByBuildingId;

public record GetProjectMonitoringByBuildingIdQuery(string Id) : IRequest<GetProjectMonitoringByIdResult>;
internal sealed class GetProjectMonitoringByBuildingIdHandler : IRequestHandler<GetProjectMonitoringByBuildingIdQuery, GetProjectMonitoringByIdResult>
{
    private readonly IReadRepository _repository;

    public GetProjectMonitoringByBuildingIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetProjectMonitoringByIdResult> Handle(GetProjectMonitoringByBuildingIdQuery request, CancellationToken cancellationToken)
    {
        var asset = _repository.AssetsView.FirstOrDefault(x => x.BuildingId == request.Id)
            ?? throw new AppException("Asset not found");

        var entity = await _repository.ProjectMonitoringView
            .Include(x => x.ProjectMonitoringBuildingComponents)
            .Where(x => x.AssetId == asset.Id)
            .Select(ProjectMonitoringMappers.MapToModelExpression())
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new AppException("Project Monitoring not found");

        return new GetProjectMonitoringByIdResult(entity);
    }
}
