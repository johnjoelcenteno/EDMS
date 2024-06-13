using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;

public class RequestedRecord
{
    private RequestedRecord() { }

    public static RequestedRecord Create(Guid recordRequestId, Guid recordTypeId, string recordType)
    {
        var entity = new RequestedRecord
        {
            RecordRequestId = recordRequestId,
            RecordTypeId = recordTypeId,
            RecordType = recordType
        };

        return entity;
    }
    public Guid Id { get; private set; }
    
    [ForeignKey(nameof(RecordRequestId))]
    public Guid RecordRequestId { get; private set; }
    /// <summary>
    /// Id from DataLibraries - RecordTypes
    /// </summary>    
    [ForeignKey(nameof(RecordTypeId))]
    public Guid RecordTypeId { get; private set; }
    public string RecordType { get; private set; }
}
