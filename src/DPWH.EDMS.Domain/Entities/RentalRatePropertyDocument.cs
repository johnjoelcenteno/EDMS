using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;

public class RentalRatePropertyDocument : EntityBase
{
    private RentalRatePropertyDocument()
    {
    }

    private RentalRatePropertyDocument(Guid id, Guid rentalRatePropertyId, string name, string? fileName, string? category, string? uri, long? fileSize)
    {
        Id = id;
        RentalRatePropertyId = rentalRatePropertyId;
        Name = name;
        Filename = fileName;
        Uri = uri;
        FileSize = fileSize;
        Category = AssetDocumentCategory.File.ToString();
    }

    public static RentalRatePropertyDocument Create(Guid id, Guid rentalRateId, string? name, string? fileName, string? category, string? uri, long? fileSize, string createdBy)
    {
        var entity = new RentalRatePropertyDocument(id, rentalRateId, name, fileName, category, uri, fileSize);
        entity.SetCreated(createdBy);

        return entity;
    }

    public void Update(string? fileName, string? name, string? uri, long? fileSize, string modifiedBy)
    {
        Filename = fileName;
        Name = name;
        Uri = uri;
        FileSize = fileSize;

        SetModified(modifiedBy);
    }

    [ForeignKey("RentalRatePropertyId")]
    public Guid RentalRatePropertyId { get; set; }
    public string? Name { get; set; }
    public string? Filename { get; set; }
    public string? Category { get; set; }
    public string? Uri { get; set; }
    public long? FileSize { get; set; }
}
