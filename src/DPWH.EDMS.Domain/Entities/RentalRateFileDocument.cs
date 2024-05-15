using DPWH.EDMS.Domain.Enums;

namespace DPWH.EDMS.Domain.Entities;

public class RentalRateFileDocument : RentalRateDocument
{
    private RentalRateFileDocument()
    {
    }
    private RentalRateFileDocument(Guid id, Guid rentalRateId, string name, string? fileName, string? uri, long? fileSize, string createdBy)
    {
        Id = id;
        RentalRateId = rentalRateId;
        Name = name;
        Filename = fileName;
        Uri = uri;
        FileSize = fileSize;
        Category = AssetDocumentCategory.File.ToString();

        SetCreated(createdBy);
    }

    public static RentalRateFileDocument Create(Guid id, Guid rentalRateId, string? name, string? fileName, string? uri, long? fileSize, string createdBy)
    {
        return new RentalRateFileDocument(id, rentalRateId, name, fileName, uri, fileSize, createdBy);
    }

    public void Update(string? name, string? fileName, string? uri, long? fileSize, string modifiedBy)
    {
        Name = name;
        Filename = fileName;
        Uri = uri;
        FileSize = fileSize;
        SetModified(modifiedBy);
    }

}
