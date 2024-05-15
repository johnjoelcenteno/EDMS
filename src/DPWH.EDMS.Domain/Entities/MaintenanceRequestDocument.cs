using DPWH.EDMS.Domain.Common;
using DPWH.EDMS.Domain.Enums;

namespace DPWH.EDMS.Domain.Entities;

public class MaintenanceRequestDocument : EntityBase
{
    private MaintenanceRequestDocument(Guid id, Guid maintenanceRequestId, string name, string group, string filename, string category, long? fileSize, string uri, string createdBy)
    {
        Id = id;
        MaintenanceRequestId = maintenanceRequestId;
        Name = name;
        Group = group.ToString();
        Filename = filename;
        Category = category;
        FileSize = fileSize;
        Uri = uri;

        SetCreated(createdBy);
    }

    public static MaintenanceRequestDocument Create(Guid id, Guid maintenanceRequestId, string name, MaintenanceDocumentType group, string filename, string category, long? fileSize, string uri, string createdBy)
    {
        return new MaintenanceRequestDocument(id, maintenanceRequestId, name, group.ToString(), filename, category, fileSize, uri, createdBy);
    }

    public void Update(string name, string filename, string category, string group, long? fileSize, string uri, string modifiedBy)
    {
        Name = name;
        Filename = filename;
        Category = category;
        Group = group.ToString();
        FileSize = fileSize;
        Uri = uri;

        SetModified(modifiedBy);
    }

    public Guid MaintenanceRequestId { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
    public string Filename { get; set; }
    public string Category { get; set; }
    public long? FileSize { get; set; }
    public string Uri { get; set; }
}
