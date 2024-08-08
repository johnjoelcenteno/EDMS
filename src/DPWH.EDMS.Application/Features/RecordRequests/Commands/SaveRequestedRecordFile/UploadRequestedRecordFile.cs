using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.SaveRequestedRecordFile;
public record struct UploadRequestedRecordFile([FromForm] IFormFile? Document, [FromForm] Guid Id, [FromForm] string? DocumentType);
