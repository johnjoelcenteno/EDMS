namespace DPWH.EDMS.Application.Models;
public class AuditableModel
{
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

}
