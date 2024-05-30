using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain;

public class DocumentRecord : EntityBase
{
    private DocumentRecord()
    {

    }

    public DocumentRecord Create(string title)
    {
        return new DocumentRecord { Id = Guid.NewGuid(), Title = title };
    }

    private string Title { get; set; }
}
