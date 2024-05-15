using DPWH.EDMS.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Assets.Commands.DeleteAsset;

public record DeleteAssetCommand(Guid AssetId) : IRequest<Guid>;

internal sealed class DeleteAssetHandler : IRequestHandler<DeleteAssetCommand, Guid>
{
    private readonly IWriteRepository _repository;

    public DeleteAssetHandler(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await _repository
            .Assets
            .SingleOrDefaultAsync(a => a.Id == request.AssetId, cancellationToken);

        if (asset is null)
        {
            return request.AssetId;
        }

        _repository.Assets.Remove(asset);
        await _repository.SaveChangesAsync(cancellationToken);

        return request.AssetId;
    }
}