using DPWH.EDMS.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.SaveUploadedFile;
public record struct UploadRecordRequestFile([FromForm] IFormFile? Document, [FromForm] RecordRequestProvidedDocumentTypes DocumentType, [FromForm] Guid DocumentTypeId);

