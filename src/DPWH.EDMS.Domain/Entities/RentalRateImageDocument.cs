using DPWH.EDMS.Domain.Enums;

namespace DPWH.EDMS.Domain.Entities;

public class RentalRateImageDocument : RentalRateDocument
{
    private RentalRateImageDocument()
    {
    }

    private RentalRateImageDocument(Guid id, Guid rentalRateId, string name, string? fileName, string? group, string? category, string? uri, long? fileSize, string createdBy)
    {
        Id = id;
        RentalRateId = rentalRateId;
        Name = name;
        Filename = fileName;
        Uri = uri;
        FileSize = fileSize;
        Category = AssetDocumentCategory.Image.ToString();
        Group = group.ToString();

        SetCreated(createdBy);
    }

    public static RentalRateImageDocument Create(Guid id, Guid rentalRateId, string? name, string? fileName, string? group, string? category, string? uri, long? fileSize, string createdBy)
    {
        return new RentalRateImageDocument(id, rentalRateId, name, fileName, group, category, uri, fileSize, createdBy);
    }

    public void Update(string? fileName, string? name, string? group, string? uri, long? fileSize)
    {
        Filename = fileName;
        Name = name;
        Group = group;
        Uri = uri;
        FileSize = fileSize;
    }
}
