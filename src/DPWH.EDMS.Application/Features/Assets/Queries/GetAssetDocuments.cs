using MediatR;

namespace DPWH.EDMS.Application.Features.Assets.Queries;

public record GetAssetDocuments(Guid AssetId) : IRequest<IEnumerable<AssetDocumentModel>>;
public record GetAssetImages(Guid AssetId) : IRequest<IEnumerable<AssetImageModel>>;
public record GetAssetFiles(Guid AssetId) : IRequest<IEnumerable<AssetFileModel>>;

public record GetAssetDocument(Guid AssetDocumentId) : IRequest<AssetDocumentModel>;
public record GetAssetImage(Guid Id) : IRequest<AssetImageModel?>;
public record GetAssetFile(Guid Id) : IRequest<AssetFileModel>;