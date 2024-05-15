using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Assets.Mappers;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssetsByBuildingId;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Queries.GetAssetApprovedPriorityListing;

public record GetAssetApprovedPriorityListingQuery(string BuildingId) : IRequest<GetAssetsByBuildingIdResult>;
internal sealed class GetAssetApprovedPriorityListingHandler : IRequestHandler<GetAssetApprovedPriorityListingQuery, GetAssetsByBuildingIdResult>
{
    private readonly IReadRepository _repository;

    public GetAssetApprovedPriorityListingHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetAssetsByBuildingIdResult> Handle(GetAssetApprovedPriorityListingQuery request, CancellationToken cancellationToken)
    {
        var asset = _repository.AssetsView.FirstOrDefault(x => x.BuildingId == request.BuildingId)
            ?? throw new AppException("Asset not found");

        var inspectionRequest = _repository.InspectionRequestsView.FirstOrDefault(x => x.AssetId == asset.Id && x.Status == "Approved" && x.Purpose == "Priority List Inspection")
            ?? throw new AppException("Asset with approved IR Priority List Inspection not found");

        var model = AssetMappers.MapToModel(asset);
        return new GetAssetsByBuildingIdResult(model);
    }
}
