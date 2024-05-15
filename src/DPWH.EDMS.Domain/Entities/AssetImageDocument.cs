using DPWH.EDMS.Domain.Enums;

namespace DPWH.EDMS.Domain.Entities;

public class AssetImageDocument : AssetDocument
{
    private AssetImageDocument() { }

    private AssetImageDocument(Guid id, Guid assetId, string filename, AssetImageView imageView, string? description,
        string? uri, double? longDegrees, double? longMinutes, double? longSeconds, string? longDirection,
        double? latDegrees, double? latMinutes, double? latSeconds, string? latDirection, long? fileSize,
        string createdBy)
    {
        Id = id;
        AssetId = assetId;
        Filename = filename;
        DocumentType = AssetDocumentCategory.Image.ToString();
        Description = description;
        Uri = uri;
        LongDegrees = longDegrees;
        LongMinutes = longMinutes;
        LongSeconds = longSeconds;
        LongDirection = longDirection;
        LatDegrees = latDegrees;
        LatMinutes = latMinutes;
        LatSeconds = latSeconds;
        LatDirection = latDirection;
        FileSize = fileSize;
        View = imageView.ToString();

        SetCreated(createdBy);
    }

    public static AssetImageDocument Create(Guid assetId, AssetImageView imageView, string createdBy)
    {
        return new AssetImageDocument(Guid.NewGuid(), assetId, imageView.ToString(), imageView, null, null, null, null, null, null, null, null, null, null, 0L, createdBy);
    }

    public static AssetImageDocument Create(Guid id, Guid assetId, string filename, AssetImageView imageView, string? description,
        string? uri, double? longDegrees, double? longMinutes, double? longSeconds, string? longDirection, double? latDegrees, double? latMinutes, double? latSeconds, string? latDirection, long? fileSize, string createdBy)
    {
        return new AssetImageDocument(id, assetId, filename, imageView, description, uri, longDegrees, longMinutes, longSeconds, longDirection, latDegrees, latMinutes, latSeconds, latDirection, fileSize, createdBy);
    }

    //public double? Longitude { get; set; }
    #region LongitudeFormat 
    public double? LongDegrees { get; set; }
    public double? LongMinutes { get; set; }
    public double? LongSeconds { get; set; }
    public string? LongDirection { get; set; }
    #endregion
    //public double? Latitude { get; set; }
    #region LatitudeFormat 
    public double? LatDegrees { get; set; }
    public double? LatMinutes { get; set; }
    public double? LatSeconds { get; set; }
    public string? LatDirection { get; set; }
    #endregion
    public string? View { get; set; }

    public void Update(string filename, string? description, AssetImageView imageView, string? uri, double? longDegrees, double? longMinutes,
        double? longSeconds, string? longDirection, double? latDegrees, double? latMinutes, double? latSeconds, string? latDirection, long? fileSize, string updatedBy)
    {
        Filename = filename;
        DocumentType = AssetDocumentCategory.Image.ToString();
        Description = description;
        Uri = uri;
        LongDegrees = longDegrees;
        LongMinutes = longMinutes;
        LongSeconds = longSeconds;
        LongDirection = longDirection;
        LatDegrees = latDegrees;
        LatMinutes = latMinutes;
        LatSeconds = latSeconds;
        LatDirection = latDirection;
        FileSize = fileSize;
        View = imageView.ToString();

        SetModified(updatedBy);
    }
}
