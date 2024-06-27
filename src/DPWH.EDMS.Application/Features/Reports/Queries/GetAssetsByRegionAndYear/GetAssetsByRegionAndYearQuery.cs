using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Reports.Queries.GetAssetsByRegionAndYear;

public record GetAssetsByRegionAndYearQuery(string RegionId, int Year) : IRequest<GetAssetsByRegionAndYearResult>;

internal sealed class GetAssetsByRegionAndYearHandler : IRequestHandler<GetAssetsByRegionAndYearQuery, GetAssetsByRegionAndYearResult>
{
    private readonly IReadRepository _repository;

    public GetAssetsByRegionAndYearHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetAssetsByRegionAndYearResult> Handle(GetAssetsByRegionAndYearQuery request, CancellationToken cancellationToken)
    {
        var region = await _repository.GeolocationsView
            .FirstOrDefaultAsync(a => a.MyId == request.RegionId, cancellationToken);

        if (region is null)
        {
            throw new AppException($"Region with Id `{request.RegionId}` not found");
        }

        var assets = await _repository.AssetsView
            .Include(a => a.FinancialDetails)
            .Where(a => a.RegionId == request.RegionId && a.FinancialDetails.EffectivityStart.Year == request.Year)
            .ToListAsync(cancellationToken);

        return new GetAssetsByRegionAndYearResult(region.Name, assets);
    }
}