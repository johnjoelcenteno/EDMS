using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain;

public class RecordType : EntityBase
{
    private RecordType() { }

    private RecordType(Guid dataLibraryId, string division, string section, string createdBy)
    {
        Id = Guid.NewGuid();
        DataLibraryId = dataLibraryId;
        Division = division;
        Section = section;
        SetCreated(createdBy);
    }

    public static RecordType Create(Guid dataLibraryId, string division, string section, string createdBy)
    {
        var mapping = new RecordType(dataLibraryId, division, section, createdBy);
        return mapping;
    }

    public void Update(Guid dataLibraryId, string division, string section, string modifiedBy)
    {
        DataLibraryId = dataLibraryId;
        Division = division;
        Section = section;
        SetModified(modifiedBy);
    }

    public Guid DataLibraryId { get; private set; }
    public string Division { get; private set; }
    public string Section { get; private set; }
}