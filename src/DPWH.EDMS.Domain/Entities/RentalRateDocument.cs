using DPWH.EDMS.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;

public abstract class RentalRateDocument : EntityBase
{
    [ForeignKey("RentalRateId")]
    public Guid RentalRateId { get; set; }
    public string? Name { get; set; }
    public string? Filename { get; set; }
    public string? Category { get; set; }
    public string? Group { get; set; }
    public string? Uri { get; set; }
    public long? FileSize { get; set; }
}
