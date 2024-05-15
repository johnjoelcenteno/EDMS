using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.Maintenance.Commands.CreateMaintenanceRequestDocument;

public class CreateMaintenanceRequestDocumentRequest
{
    [Required]
    public IFormFile File { get; set; }
    [Required]
    public Guid MaintenanceRequestId { get; set; }
    public Guid? Id { get; set; }
    public string? Filename { get; set; }
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Group { get; set; }
    public string? Uri { get; set; }
}