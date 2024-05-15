using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class InspectionRequestProjectMonitoringScopesImage : EntityBase
{
    private InspectionRequestProjectMonitoringScopesImage()
    {
    }

    private InspectionRequestProjectMonitoringScopesImage(Guid id, Guid projectMonitoringId, string fileName, long fileSize, string uri, string createdBy)
    {
        Id = id;
        InspectionRequestProjectMonitoringScopeId = projectMonitoringId;
        Filename = fileName;
        Uri = uri;
        FileSize = fileSize;
        SetCreated(createdBy);
    }

    public static InspectionRequestProjectMonitoringScopesImage Create(Guid id, Guid projectMonitoringId, string fileName, long fileSize, string uri, string createdBy)
    {
        return new InspectionRequestProjectMonitoringScopesImage(id, projectMonitoringId, fileName, fileSize, uri, createdBy);
    }

    public void Update(string fileName, long fileSize, string uri, string modifiedBy)
    {
        Filename = fileName;
        FileSize = fileSize;
        Uri = uri;
        SetModified(modifiedBy);
    }

    public string Filename { get; set; }
    public long? FileSize { get; set; }
    public string Uri { get; set; }
    public Guid InspectionRequestProjectMonitoringScopeId { get; set; }
}
