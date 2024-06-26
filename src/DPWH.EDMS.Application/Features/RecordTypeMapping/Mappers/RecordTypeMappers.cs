using DPWH.EDMS.Domain;

namespace DPWH.EDMS.Application;

public static class RecordTypeMappingMappers
{
    public static QueryRecordTypesModel Map(RecordType recordType)
    {
        return new QueryRecordTypesModel
        {
            DataLibraryId = recordType.DataLibraryId,
            Division = recordType.Division,
            Section = recordType.Section,
        };
    }
}
