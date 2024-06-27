using DPWH.EDMS.Domain;

namespace DPWH.EDMS.Application;

public static class RecordTypeMappingMappers
{
    public static QueryRecordTypesModel Map(RecordType recordType)
    {
        return new QueryRecordTypesModel
        {
            Name = recordType.Name,
            Category = recordType.Category,
            Section = recordType.Section,
            Office = recordType.Office,
            IsActive = recordType.IsActive,
        };
    }
}
