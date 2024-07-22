using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DPWH.EDMS.Application.Features.RecordRequests.Commands.SaveTransmittalReceipt;

public record struct UploadTransmittalReceipt([FromForm] IFormFile? Document, [FromForm] Guid RecordRequestId, DateTimeOffset DateReceived, DateTimeOffset TimeReceived);