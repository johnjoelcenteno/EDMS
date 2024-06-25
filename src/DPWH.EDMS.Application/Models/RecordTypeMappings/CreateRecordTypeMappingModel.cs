using DPWH.EDMS.Application.Models;

namespace DPWH.EDMS.Application;

public class CreateRecordTypeMappingModel
{
    public Guid DataLibraryId { get; private set; }
    public string Division { get; private set; }
    public string Section { get; private set; }
}
