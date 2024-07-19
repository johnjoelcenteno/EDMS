using DPWH.EDMS.Domain;

namespace DPWH.EDMS.Application.Features.RecordTypes.Mappers;

public static class RecordTypeMappers
{
    public static QueryRecordTypesModel Map(RecordType recordType)
    {
        return new QueryRecordTypesModel
        {
            Id = recordType.Id,
            Name = recordType.Name,
            Category = recordType.Category,
            Section = recordType.Section ?? default!,
            Office = recordType.Office ?? default!,
            Created = recordType.Created,
            CreatedBy = recordType.CreatedBy,
            IsActive = recordType.IsActive,
            Code = recordType.Code ?? default!,
        };
    }
}
