namespace DPWH.EDMS.Domain;

public class DocumentRecords
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset Created { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset Modified { get; set; }
    public string ModifiedBy { get; set; }
}
