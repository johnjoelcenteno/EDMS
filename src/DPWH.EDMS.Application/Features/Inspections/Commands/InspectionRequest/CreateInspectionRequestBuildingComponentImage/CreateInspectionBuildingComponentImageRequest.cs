using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequestBuildingComponentImage;

public class SaveInspectionRequestBuildingComponentImageRequest
{
    [Required]
    public IFormFile File { get; set; }
    public Guid? Id { get; set; }
    public Guid InspectionRequestBuildingComponentId { get; set; }
    public string Filename { get; set; }
    public string? Uri { get; set; }
}
