using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using UUIDNext;

namespace DPWH.EDMS.Domain.Entities;

public class RequestedRecord : EntityBase
{
    private RequestedRecord() { }

    public static RequestedRecord Create(Guid recordRequestId, Guid recordTypeId, string recordType, string office)
    {
        var entity = new RequestedRecord
        {
            Id = Uuid.NewDatabaseFriendly(Database.SqlServer),
            RecordRequestId = recordRequestId,
            RecordTypeId = recordTypeId,
            RecordType = recordType,
            Office = office,
            Status = RequestedRecordStatus.Pending.ToString()
        };
        entity.SetCreated("RecordRequest");
        return entity;
    }

    public void UpdateIsAvailable(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
    public void Update(string uri)
    {
        Uri = uri;
    }
    public void UpdateDocumentType(string documentype)
    {
        DocumentType = documentype;
    }
    public void UpdateDocumentStatus(RequestedRecordStatus status)
    {
        Status = status.ToString();

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
    public string Office { get; private set; }
    public string Status { get; private set; }
    public bool IsAvailable { get; private set; }
    public string? DocumentType { get; private set; }
    public string? Uri { get; private set; }

}
