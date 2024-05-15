using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.CreateProjectMonitoringBuildingComponentImage;

public class CreateProjectMonitoringBuildingComponentImageRequest
{
    [Required]
    public IFormFile File { get; set; }
    public Guid? Id { get; set; }
    public Guid ProjectMonitoringBuildingComponentId { get; set; }
    public string Filename { get; set; }
    public string? Uri { get; set; }
}
