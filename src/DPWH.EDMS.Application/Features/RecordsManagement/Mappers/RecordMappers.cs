using DPWH.EDMS.Application.Features.RecordsManagement.Queries;
using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application.Features.RecordsManagement.Mappers;

public static class RecordMappers
{
    public static RecordModel MapToModel(Record entity)
    {
        return new RecordModel
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            RecordName = entity.RecordName,
            RecordTypeId = entity.RecordTypeId,
            DocVersion = entity.Created.ToString("MMddyyyy")
        };
    }
}
