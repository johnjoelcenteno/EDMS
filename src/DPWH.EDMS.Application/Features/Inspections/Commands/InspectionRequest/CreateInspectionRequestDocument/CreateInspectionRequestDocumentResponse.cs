using DPWH.EDMS.Application.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DPWH.EDMS.Application.Features.Inspections.Commands.InspectionRequest.CreateInspectionRequestDocument;

public class SaveInspectionRequestDocumentResponse : IdResponse
{
    public SaveInspectionRequestDocumentResponse(Guid id) : base(id) { }
}

public class SaveInspectionRequestDocument : IRequest<SaveInspectionRequestDocumentResponse>
{
    public SaveInspectionRequestDocument(CreateInspectionRequestDocumentRequest request)
    {
        Details = request;
    }

    [Required]
    public CreateInspectionRequestDocumentRequest Details { get; }
}