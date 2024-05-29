using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain;

public class DocumentRecord : EntityBase
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}
