using DPWH.EDMS.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;
public abstract class AssetDocument : EntityBase
{
    [ForeignKey("AssetId")]
    public Guid AssetId { get; set; }
    public virtual Asset Asset { get; set; }
    public string? Filename { get; set; }
    /// <summary>
    /// See Enums.AssetDocumentTypes
    /// </summary>
    /// <example>Title;TaxDeclaration;Image</example>
    public string DocumentType { get; set; }
    public string Category { get; set; }
    public string? Description { get; set; }
    public string? Uri { get; set; }
    public long? FileSize { get; set; }
}
