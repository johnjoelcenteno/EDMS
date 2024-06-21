namespace DPWH.EDMS.Application.Features.RecordRequests.Queries;
public class RecordRequestDocumentModel
{
    public Guid? Id { get; set; }
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
