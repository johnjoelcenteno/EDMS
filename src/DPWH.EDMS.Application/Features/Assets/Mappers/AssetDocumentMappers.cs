using DPWH.EDMS.Application.Features.Assets.Queries;
using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.Assets.Mappers;
public static class AssetDocumentMappers
{
    public static IEnumerable<AssetDocumentModel> MapToModel(IEnumerable<AssetDocument> entities)
    {
        return entities.Select(MapToModel);
    }

    public static IEnumerable<AssetImageModel> MapToModel(IEnumerable<AssetImageDocument> entities)
    {
        return entities.Select(MapToModel);
    }

    public static IEnumerable<AssetFileModel> MapToModel(IEnumerable<AssetFileDocument> entities)
    {
        return entities.Select(MapToModel);
    }


    public static AssetImageModel MapToModel(AssetImageDocument entityImage)
    {
        return new AssetImageModel
        {
            Id = entityImage.Id,
            AssetId = entityImage.AssetId,
            Filename = entityImage.Filename,
            Category = entityImage.Category,
            DocumentType = entityImage.DocumentType,
            Description = entityImage.Description,
            Uri = entityImage.Uri,
            Longitude = new LongLatFormat
            {
                Degrees = entityImage.LongDegrees,
                Minutes = entityImage.LongMinutes,
                Seconds = entityImage.LongSeconds,
                Direction = entityImage.LongDirection
            },
            Latitude = new LongLatFormat
            {
                Degrees = entityImage.LatDegrees,
                Minutes = entityImage.LatMinutes,
                Seconds = entityImage.LatSeconds,
                Direction = entityImage.LatDirection

            },
            FileSize = entityImage.FileSize,
            View = entityImage.View,
            Created = entityImage.Created,
            CreatedBy = entityImage.CreatedBy,
            LastModified = entityImage.LastModified,
            LastModifiedBy = entityImage.LastModifiedBy
        };
    }

    public static AssetFileModel MapToModel(AssetFileDocument entityFile)
    {
        return new AssetFileModel
        {
            Id = entityFile.Id,
            AssetId = entityFile.AssetId,
            Filename = entityFile.Filename,
            Category = entityFile.Category,
            DocumentType = entityFile.DocumentType,
            DocumentTypeOthers = entityFile.DocumentTypeOthers,
            OtherRelatedDocuments = entityFile.OtherRelatedDocuments,
            Description = entityFile.Description,
            Uri = entityFile.Uri,
            FileSize = entityFile.FileSize,
            Created = entityFile.Created,
            CreatedBy = entityFile.CreatedBy,
            LastModified = entityFile.LastModified,
            LastModifiedBy = entityFile.LastModifiedBy
        };
    }

    public static AssetDocumentModel MapToModel(AssetDocument entity)
    {
        if (entity is AssetImageDocument entityImage)
        {
            return new AssetImageModel
            {
                Id = entityImage.Id,
                AssetId = entityImage.AssetId,
                Filename = entityImage.Filename,
                Category = entityImage.Category,
                DocumentType = entityImage.DocumentType,
                Description = entityImage.Description,
                Uri = entityImage.Uri,
                Longitude = new LongLatFormat
                {
                    Degrees = entityImage.LongDegrees,
                    Minutes = entityImage.LongMinutes,
                    Seconds = entityImage.LongSeconds,
                    Direction = entityImage.LongDirection
                },
                Latitude = new LongLatFormat
                {
                    Degrees = entityImage.LatDegrees,
                    Minutes = entityImage.LatMinutes,
                    Seconds = entityImage.LatSeconds,
                    Direction = entityImage.LatDirection

                },
                FileSize = entityImage.FileSize,
                View = entityImage.View,
                Created = entityImage.Created,
                CreatedBy = entityImage.CreatedBy,
                LastModified = entityImage.LastModified,
                LastModifiedBy = entityImage.LastModifiedBy
            };
        }
        if (entity is AssetFileDocument entityFile)
        {
            return new AssetFileModel
            {
                Id = entityFile.Id,
                AssetId = entityFile.AssetId,
                Filename = entityFile.Filename,
                Category = entityFile.Category,
                DocumentType = entityFile.DocumentType,
                DocumentTypeOthers = entityFile.DocumentTypeOthers,
                OtherRelatedDocuments = entityFile.OtherRelatedDocuments,
                Description = entityFile.Description,
                Uri = entityFile.Uri,
                FileSize = entityFile.FileSize,
                Created = entityFile.Created,
                CreatedBy = entityFile.CreatedBy,
                LastModified = entityFile.LastModified,
                LastModifiedBy = entityFile.LastModifiedBy
            };
        }

        return null;
    }
}
