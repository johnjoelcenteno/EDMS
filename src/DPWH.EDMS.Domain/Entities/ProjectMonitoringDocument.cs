using DPWH.EDMS.Domain.Common;

namespace DPWH.EDMS.Domain.Entities;

public class ProjectMonitoringDocument : EntityBase
{

    private ProjectMonitoringDocument(Guid id, Guid projectMonitoringId, string name, string group, string filename, string category, long? fileSize, string uri, string createdBy)
    {
        Id = id;
        ProjectMonitoringId = projectMonitoringId;
        Name = name;
        Group = group.ToString();
        Filename = filename;
        Category = category;
        FileSize = fileSize;
        Uri = uri;

        SetCreated(createdBy);
    }

    public static ProjectMonitoringDocument Create(Guid id, Guid projectMonitoringId, string name, string group, string filename, string category, long? fileSize, string uri, string createdBy)
    {
        return new ProjectMonitoringDocument(id, projectMonitoringId, name, group, filename, category, fileSize, uri, createdBy);
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

    public Guid ProjectMonitoringId { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
    public string Filename { get; set; }
    public string Category { get; set; }
    public long? FileSize { get; set; }
    public string Uri { get; set; }
}
