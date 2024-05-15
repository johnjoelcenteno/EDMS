using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class InspectionRequestBuildingComponentImage : EntityBase
{
    private InspectionRequestBuildingComponentImage()
    {
    }

    private InspectionRequestBuildingComponentImage(Guid id, Guid inspectionRequestBuildingComponentId, string filename, long? fileSize, string uri, string createdBy)
    {
        Id = id;
        Filename = filename;
        FileSize = fileSize;
        Uri = uri;
        InspectionRequestBuildingComponentId = inspectionRequestBuildingComponentId;
        SetCreated(createdBy);
    }

    public static InspectionRequestBuildingComponentImage Create(Guid id, Guid inspectionRequestBuildingComponentId, string fileName, long? fileSize, string uri, string createdBy)
    {
        return new InspectionRequestBuildingComponentImage(id, inspectionRequestBuildingComponentId, fileName, fileSize, uri, createdBy);
    }

    public void Update(string fileName, long? fileSize, string uri, string modifiedBy)
    {
        Filename = fileName;
        FileSize = fileSize;
        Uri = uri;

        SetModified(modifiedBy);
    }

    public string Filename { get; set; }
    public long? FileSize { get; set; }
    public string Uri { get; set; }
    public Guid InspectionRequestBuildingComponentId { get; set; }
    public InspectionRequestBuildingComponent InspectionRequestBuildingComponent { get; set; }
}
