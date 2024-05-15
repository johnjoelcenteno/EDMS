using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesPropertyDocument;

public class CreateRentalRatesPropertyDocumentRequest
{
    public Guid? Id { get; set; }
    [Required]
    public IFormFile File { get; set; }
    [Required]
    public Guid RentalRatesPropertyId { get; set; }
    public string? Filename { get; set; }
    public string? Name { get; set; }
    public string? Uri { get; set; }
    public string? Category { get; set; }
}
