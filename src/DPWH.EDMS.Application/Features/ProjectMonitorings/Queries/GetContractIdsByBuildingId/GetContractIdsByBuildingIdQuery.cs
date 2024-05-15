using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetContractIdsByBuildingId;

public record GetContractIdsByBuildingIdQuery(string BuildingId) : IRequest<GetContractIdByBuildingIdResult>;
internal sealed class GetContractIdsByBuildingIdHandler : IRequestHandler<GetContractIdsByBuildingIdQuery, GetContractIdByBuildingIdResult>
{
    public readonly IReadRepository _repository;

    public GetContractIdsByBuildingIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetContractIdByBuildingIdResult> Handle(GetContractIdsByBuildingIdQuery request, CancellationToken cancellationToken)
    {
        var asset = _repository.AssetsView.FirstOrDefault(x => x.BuildingId == request.BuildingId);

        var projectmonitorings = _repository.ProjectMonitoringView.Where(x => x.AssetId == asset.Id).ToList();

        return new GetContractIdByBuildingIdResult(projectmonitorings);
    }
}
