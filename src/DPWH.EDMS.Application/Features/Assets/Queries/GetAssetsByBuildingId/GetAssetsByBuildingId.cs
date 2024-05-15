using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DPWH.EDMS.Domain.Exceptions;
using DPWH.EDMS.Domain.Entities;
using DPWH.EDMS.Application.Features.Assets.Mappers;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Assets.Queries.GetAssetsByImplementingOffice;

namespace DPWH.EDMS.Application.Features.Assets.Queries.GetAssetsByBuildingId;

public record GetAssetsByBuildingId(string Id, string? Purpose) : IRequest<GetAssetsByBuildingIdResult>;

internal sealed class GetAssetsByBuildingIdHandler : IRequestHandler<GetAssetsByBuildingId, GetAssetsByBuildingIdResult>
{
    private readonly IReadRepository _repository;
    private readonly ClaimsPrincipal _principal;
    private readonly ILogger<GetAssetsByImplementingOfficeHandler> _logger;

    public GetAssetsByBuildingIdHandler(IReadRepository repository, ClaimsPrincipal principal, ILogger<GetAssetsByImplementingOfficeHandler> logger)
    {
        _repository = repository;
        _principal = principal;
        _logger = logger;
    }

    public async Task<GetAssetsByBuildingIdResult> Handle(GetAssetsByBuildingId request, CancellationToken cancellationToken)
    {
        var asset = await _repository.AssetsView
            .Where(asset => asset.BuildingId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (request.Purpose == "Project Monitoring" || request.Purpose == "Priority List Inspection")
        {
            if (asset is null)
            {
                throw new AppException("No asset available");
            }
            else if (asset.PropertyStatus != "Minor Repair" && asset.PropertyStatus != "Major Repair")
            {
                throw new AppException("Asset is not in need of repair");
            }
        }
        var model = AssetMappers.MapToModel(asset);
        return new GetAssetsByBuildingIdResult(model);
    }
}
