using System.Reflection;
using System.Text.Json;
using DPWH.EDMS.Application.Contracts.Persistence;
using DPWH.EDMS.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DPWH.EDMS.Application.Features.Assets.Commands.PatchAsset;

public record PatchAssetCommand(Guid AssetId, PatchAssetRequest Asset) : IRequest<Guid>;

internal sealed class PatchAssetHandler : IRequestHandler<PatchAssetCommand, Guid>
{
    private readonly IWriteRepository _repository;

    public PatchAssetHandler(IWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(PatchAssetCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository
            .Assets
            .SingleOrDefaultAsync(a => a.Id == request.AssetId, cancellationToken);

        if (entity is null)
        {
            throw new AppException("Property not found");
        }

        var oldEntity = entity.Clone();
        if (entity.Equals(oldEntity))
        {
            return request.AssetId;
        }

        var patchDocument = request.Asset;
        foreach (var patchOperation in patchDocument.Operations)
        {
            var propertyName = patchOperation.PropertyName;
            var propertyInfo = entity.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo is null)
            {
                continue;
            }

            var propertyValue = JsonSerializer.Deserialize(patchOperation.Value.GetRawText(), propertyInfo.PropertyType);
            propertyInfo.SetValue(entity, propertyValue);
        }

        _repository.Assets.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}