using DPWH.EDMS.Application.Features.RecordRequests.Queries;
using DPWH.EDMS.Domain.Entities;

namespace DPWH.EDMS.Application;

public static class RecordRequestDocumentMapper
{
    public static RecordRequestDocumentModel Map(RecordRequestDocument record)
    {
        return new RecordRequestDocumentModel
        {
            Id = record.Id,
            Name = record.Name,
            Filename = record.Filename,
            Type = record.Type,
            DocumentTypeId = record.DocumentTypeId,
            FileSize = record.FileSize,
            Uri = record.Uri
        };
    }
}
