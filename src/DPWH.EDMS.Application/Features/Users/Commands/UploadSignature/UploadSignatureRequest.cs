using Microsoft.AspNetCore.Mvc;
using DPWH.EDMS.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace DPWH.EDMS.Application.Features.Users.Commands.UploadSignature
{
    public record UploadSignatureRequest([FromForm] IFormFile? Signature, [FromForm] Guid SignatoriesId);
}
