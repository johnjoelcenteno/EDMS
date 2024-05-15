using MediatR;
using Microsoft.EntityFrameworkCore;
using DPWH.EDMS.Application.Features.Assets.Mappers;
using DPWH.EDMS.Application.Contracts.Persistence;

namespace DPWH.EDMS.Application.Features.Assets.Queries;

public class AssetsQueryHandler : IRequestHandler<GetAssetDocuments, IEnumerable<AssetDocumentModel>>,
                                  IRequestHandler<GetAssetImages, IEnumerable<AssetImageModel>>,
                                  IRequestHandler<GetAssetFiles, IEnumerable<AssetFileModel>>,
                                  IRequestHandler<GetAssetDocument, AssetDocumentModel?>,
                                  IRequestHandler<GetAssetImage, AssetImageModel?>,
                                  IRequestHandler<GetAssetFile, AssetFileModel?>
{

    private readonly IReadRepository _readRepository;

    public AssetsQueryHandler(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<IEnumerable<AssetDocumentModel>> Handle(GetAssetDocuments request, CancellationToken cancellationToken)
    {
        //var assetDocuments = await _readRepository.AssetDocumentsView
        //    .Where(x => x.AssetId == request.AssetId).ToListAsync(cancellationToken);

        //return assetDocuments.Any()
        //    ? AssetDocumentMappers.MapToModel(assetDocuments)
        //    : Enumerable.Empty<AssetDocumentModel>();

        return Enumerable.Empty<AssetDocumentModel>();
    }

    public async Task<IEnumerable<AssetImageModel>> Handle(GetAssetImages request, CancellationToken cancellationToken)
    {
        var assetDocuments = await _readRepository.AssetImagesView
            .Where(x => x.AssetId == request.AssetId).ToListAsync(cancellationToken);

        return assetDocuments.Any()
            ? AssetDocumentMappers.MapToModel(assetDocuments)
            : Enumerable.Empty<AssetImageModel>();
    }

    public async Task<IEnumerable<AssetFileModel>> Handle(GetAssetFiles request, CancellationToken cancellationToken)
    {
        var assetDocuments = await _readRepository.AssetFilesView
            .Where(x => x.AssetId == request.AssetId).ToListAsync(cancellationToken);

        return assetDocuments.Any()
            ? AssetDocumentMappers.MapToModel(assetDocuments)
            : Enumerable.Empty<AssetFileModel>();
    }

    public async Task<AssetDocumentModel?> Handle(GetAssetDocument request, CancellationToken cancellationToken)
    {
        var assetDocument = await _readRepository.AssetImagesView
            .FirstOrDefaultAsync(x => x.Id == request.AssetDocumentId, cancellationToken);

        if (assetDocument is not null)
        {
            return AssetDocumentMappers.MapToModel(assetDocument);
        }

        var assetFile = await _readRepository.AssetFilesView
            .FirstOrDefaultAsync(x => x.Id == request.AssetDocumentId, cancellationToken);

        if (assetDocument is not null)
        {
            return AssetDocumentMappers.MapToModel(assetDocument);
        }

        return null;
    }

    public async Task<AssetImageModel?> Handle(GetAssetImage request, CancellationToken cancellationToken)
    {
        var assetDocument = await _readRepository.AssetImagesView
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (assetDocument is not null)
        {
            return AssetDocumentMappers.MapToModel(assetDocument);
        }

        return null;
    }
    public async Task<AssetFileModel?> Handle(GetAssetFile request, CancellationToken cancellationToken)
    {
        var assetDocument = await _readRepository.AssetFilesView
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (assetDocument is not null)
        {
            return AssetDocumentMappers.MapToModel(assetDocument);
        }

        return null;
    }
}
