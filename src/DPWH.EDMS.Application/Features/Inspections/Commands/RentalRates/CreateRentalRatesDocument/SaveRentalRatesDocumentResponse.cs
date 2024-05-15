using DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesPropertyDocument;
using DPWH.EDMS.Application.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.RentalRates.CreateRentalRatesDocument;

public class SaveRentalRateDocumentResponse : IdResponse
{
    public SaveRentalRateDocumentResponse(Guid id) : base(id) { }
}

public class SaveRentalRateDocument : IRequest<SaveRentalRateDocumentResponse>
{
    public SaveRentalRateDocument(CreateRentalRatesDocumentRequest request)
    {
        Details = request;
    }

    [Required]
    public CreateRentalRatesDocumentRequest Details { get; }
}

public class SaveRentalRatePropertyDocument : IRequest<SaveRentalRateDocumentResponse>
{
    public SaveRentalRatePropertyDocument(CreateRentalRatesPropertyDocumentRequest request)
    {
        Details = request;
    }

    [Required]
    public CreateRentalRatesPropertyDocumentRequest Details { get; }
}

