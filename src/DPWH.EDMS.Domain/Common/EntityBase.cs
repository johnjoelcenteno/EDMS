namespace DPWH.EDMS.Domain.Common;

public class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }

    public DateTimeOffset? LastModified { get; protected set; }
    public string? LastModifiedBy { get; protected set; }

    protected void SetCreated(string createdBy)
    {
        CreatedBy = createdBy;
        Created = DateTimeOffset.Now;
    }

    protected void SetModified(string updatedBy)
    {
        LastModifiedBy = updatedBy;
        LastModified = DateTimeOffset.Now;
    }
}
