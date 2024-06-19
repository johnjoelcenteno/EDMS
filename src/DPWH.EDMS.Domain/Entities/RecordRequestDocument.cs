using DPWH.EDMS.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DPWH.EDMS.Domain.Entities;
/// <summary>
/// This represents uploaded document/files when creating new request.
/// This is either valid Id or Supporting document
/// </summary>

public class RecordRequestDocument : EntityBase
{
    private RecordRequestDocument(string name, string filename, string type, Guid documentTypeId, long? fileSize, string uri, string createdBy)
    {            
        Name = name;        
        Filename = filename;
        Type = type;
        DocumentTypeId = documentTypeId;
        FileSize = fileSize;
        Uri = uri;

        SetCreated(createdBy);
    }

    public static RecordRequestDocument Create(string name, string filename, string type, Guid documentTypeId, long? fileSize, string uri, string createdBy)
    {
        return new RecordRequestDocument(name, filename, type, documentTypeId, fileSize, uri, createdBy);
    }
    public void Update(string name, string filename, string type, long? fileSize, string uri, string modifiedBy)
    {
        Name = name;
        Filename = filename;
        Type = type;        
        FileSize = fileSize;
        Uri = uri;

        SetModified(modifiedBy);
    }

    public void Associate(Guid recordRequestId, string modifiedBy)
    {
        RecordRequestId = recordRequestId;
        SetModified(modifiedBy);
    }

    [ForeignKey("RecordRequestId")]
    public Guid? RecordRequestId { get; set; }
    public string? Name { get; set; }    
    public string? Filename { get; set; }
    /// <summary>
    /// Either ValidId or SupportingDocument
    /// </summary>
    public string Type { get; set; }
    public Guid DocumentTypeId { get; set; }
    public long? FileSize { get; set; }
    public string? Uri { get; set; }
}
