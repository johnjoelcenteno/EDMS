using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Application.Features.Assets.Mappers;
using DPWH.EDMS.Application.Features.Assets.Models;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Assets.Queries.GetAssetById;

public record GetAssetByIdQuery(Guid AssetId) : IRequest<AssetResponse>;

internal sealed class GetAssetByIdHandler : IRequestHandler<GetAssetByIdQuery, AssetResponse>
{
    private readonly IReadRepository _repository;

    public GetAssetByIdHandler(IReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<AssetResponse> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.AssetsView
            .Include(asset => asset.Images)
            .Include(asset => asset.Files)
            .Include(asset => asset.FinancialDetails)
            .Include(asset => asset.FinancialDetailsDocuments)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == request.AssetId, cancellationToken);

        if (entity is null)
        {
            throw new AppException($"Asset not found: `{request.AssetId}`");
        }

        var model = AssetMappers.MapToModel(entity);
        return new AssetResponse(model);
    }
}