using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Domain.Enums;

namespace DPWH.EDMS.Domain.Entities;

public class InspectionRequestDocument : EntityBase
{
    private InspectionRequestDocument(Guid id, Guid inspectionRequestId, string name, string filename, long? fileSize, string uri, string createdBy)
    {
        Id = id;
        InspectionRequestId = inspectionRequestId;
        Name = name;
        Filename = filename;
        FileSize = fileSize;
        Uri = uri;

        SetCreated(createdBy);
    }

    public static InspectionRequestDocument Create(Guid id, Guid maintenanceRequestId, string name, string filename, long? fileSize, string uri, string createdBy)
    {
        return new InspectionRequestDocument(id, maintenanceRequestId, name, filename, fileSize, uri, createdBy);
    }
    public void Update(string name, string filename, long? fileSize, string uri, string modifiedBy)
    {
        Name = name;
        Filename = filename;
        FileSize = fileSize;
        Uri = uri;

        SetModified(modifiedBy);
    }

    public Guid InspectionRequestId { get; set; }
    public string Name { get; set; }
    public string Filename { get; set; }
    public long? FileSize { get; set; }
    public string Uri { get; set; }
}
