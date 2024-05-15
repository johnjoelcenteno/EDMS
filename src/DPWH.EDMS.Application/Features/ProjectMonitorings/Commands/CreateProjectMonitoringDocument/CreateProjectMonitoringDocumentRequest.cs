using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPWH.EDMS.Application.Features.ProjectMonitorings.Commands.CreateProjectMonitoringDocument;

public class CreateProjectMonitoringDocumentRequest
{
    [Required]
    public IFormFile File { get; set; }
    [Required]
    public Guid ProjectMonitoringId { get; set; }
    public Guid? Id { get; set; }
    public string? Filename { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Group { get; set; }
    public string? Uri { get; set; }
}
