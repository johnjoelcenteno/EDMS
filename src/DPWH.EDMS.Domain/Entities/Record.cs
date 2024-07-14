using DPWH.EDMS.Domain.Common;
using UUIDNext;

namespace DPWH.EDMS.Domain.Entities;

public class Record : EntityBase
{
    private Record() { }

    public static Record Create(string employeeId, Guid recordTypeId, string recordName, string recordUri, string createdBy)
    {
        var entity = new Record
        {
            Id = Uuid.NewDatabaseFriendly(Database.SqlServer),
            EmployeeId = employeeId,
            RecordTypeId = recordTypeId,
            RecordName = recordName,
            RecordUri = recordUri
        };
        entity.SetCreated(createdBy);
        return entity;
    }    
    public string EmployeeId { get; private set; }
    public Guid RecordTypeId { get; private set; }
    public string RecordName { get; private set; }
    public string RecordUri { get; private set; }    
}
