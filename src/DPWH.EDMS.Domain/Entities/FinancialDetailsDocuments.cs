using DPWH.EDMS.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;

public class FinancialDetailsDocuments : EntityBase
{
    private FinancialDetailsDocuments() { }
    public static FinancialDetailsDocuments Create(Guid id, Guid assetId, string? yearFunded, double? allocation, string? filename, string? uri, long? fileSize, string createdBy)
    {
        var entity = new FinancialDetailsDocuments
        {
            Id = id,
            AssetId = assetId,
            YearFunded = yearFunded,
            Allocation = allocation,
            FileName = filename,
            Uri = uri,
            FileSize = fileSize
        };
        entity.SetCreated(createdBy);
        return entity;
    }

    public Guid AssetId { get; set; }
    public virtual Asset Asset { get; set; }
    public string? YearFunded { get; set; }
    public double? Allocation { get; set; }
    public string? FileName { get; set; }
    public string? Uri { get; set; }
    public long? FileSize { get; set; }

    public void Update(string? yearFunded, double? allocation, string? filename, string? uri, long? fileSize, string modifiedBy)
    {
        YearFunded = yearFunded;
        Allocation = allocation;
        FileName = filename;
        FileSize = fileSize;
        Uri = uri;

        SetModified(modifiedBy);
    }
}
