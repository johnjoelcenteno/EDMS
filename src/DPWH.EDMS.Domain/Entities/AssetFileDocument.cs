namespace DPWH.EDMS.Domain.Entities;

public class AssetFileDocument : AssetDocument
{
    private AssetFileDocument() { }
    public static AssetFileDocument Create(Guid id, Guid assetId, string? filename, string documentType, string? documentNo, string? documentTypeOthers, string? otherRelatedDocuments,
        string? description, string? uri, long? fileSize, string createdBy)
    {
        var entity = new AssetFileDocument
        {
            Id = id,
            AssetId = assetId,
            Filename = filename,
            DocumentType = documentType,
            DocumentNo = documentNo,
            DocumentTypeOthers = documentTypeOthers,
            OtherRelatedDocuments = otherRelatedDocuments,
            Description = description,
            Uri = uri,
            FileSize = fileSize
        };

        entity.SetCreated(createdBy);

        return entity;
    }

    public string? DocumentNo { get; set; }
    public string? DocumentTypeOthers { get; set; }
    public string? OtherRelatedDocuments { get; set; }

    public void Update(string? filename, string documentType, string? documentNo, string? documentTypeOthers, string? otherRelatedDocuments, string? description, string? uri, long? fileSize, string updatedBy)
    {
        Filename = filename;
        DocumentNo = documentNo;
        DocumentType = documentType;
        DocumentTypeOthers = documentTypeOthers;
        OtherRelatedDocuments = otherRelatedDocuments;
        Description = description;
        FileSize = fileSize;
        Uri = uri;

        SetModified(updatedBy);
    }
}
